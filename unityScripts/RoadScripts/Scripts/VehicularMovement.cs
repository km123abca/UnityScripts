using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VehState{
	NORMAL,SLOW,TURN,STOPPED
}
public class VehicularMovement : MonoBehaviour {

	Rigidbody2D rb;
	public float speed=5f;
	VehState hungState=VehState.SLOW;
	VehState presentState=VehState.NORMAL;

	//SIMPLE MOVEMENT PARAMETRES STARTS
	public float maxWheelAngle=0.5f;
	float wheelAngle=0f;
	public float turnSpeed=0.05f;
	public float wheelStraightenRatio=0.1f;
	//SIMPLE MOVEMENT PARAMETRES ENDS

	float tyreTilt=0f;
	public float tyreTiltSpeed=1f,maxTyreTilt=45f,slowReturnTyreTiltSpeed=1f,fastReturnTyreTiltSpeed=5f;
	public FrontTyreBehav tyreScript1,tyreScript2;
	public float tyreSeperation=1.57f;
	float pushFromEngine;
	public float steeringSpeed=10f;
	public float slowDeccVal=0.5f,accVal=3f;
	public float tyreTiltAdjFraction=0.005f;
	public float frictionFactor=0.1f;
	public float peWeightage=0.05f,ttWeightage=0.05f;


	//vars for holding starts
	bool vehichleUnderHold,holdable;
	float lastHoldClockTime;
	public float maxHoldTime=2f;
	//vars for holding ends

	//vars for raycasting and hindrance detection starts
	public float sideWaysOffset=2f,raycastLengthFront=4f;
	//vars for raycasting and hindrance detection ends

	// float rotationSpeed;

	TyreScript frontLeftTyre,rearLeftTyre,frontRightTyre;
	public float sepFromFence=0.8f;
	public bool turnLeft;
	float aiTireTiltControl=0f,hungStateWheelTurn=0f;
	public float k_push=1f;

	void Start () {
		rb=GetComponent<Rigidbody2D>();
		frontLeftTyre=transform.Find("FTL").GetComponent<TyreScript>();
		frontRightTyre=transform.Find("FTR").GetComponent<TyreScript>();
		rearLeftTyre =transform.Find("RTL").GetComponent<TyreScript>();
		
	}
	
	
	void Update () 
		{
			float horizInput=-Input.GetAxis("Horizontal");
			float vertInput = Input.GetAxis("Vertical");
			 SimulatedMovement(horizInput,vertInput);
			//AIMovement();
		}

	void SimpleMovement()
		{
			float horizInput=-Input.GetAxis("Horizontal");
			float vertInput = Input.GetAxis("Vertical");
			if(Mathf.Abs(horizInput)>0f)
				wheelAngle+=horizInput*turnSpeed;
			else if(wheelAngle!=0f)
				{
					if(wheelAngle > 0f)				
						wheelAngle-=turnSpeed*wheelStraightenRatio;
					else
						wheelAngle+=turnSpeed*wheelStraightenRatio;
					if(Mathf.Abs(wheelAngle)<= turnSpeed*wheelStraightenRatio)
					wheelAngle=0f;				
				}
			wheelAngle=Mathf.Clamp(wheelAngle,-maxWheelAngle,maxWheelAngle);	
			rb.velocity=Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*speed*vertInput;
			if(vertInput > 0f)
				transform.rotation=Quaternion.Euler(0,0,wheelAngle)*transform.rotation;
			else if(vertInput < 0f)
				transform.rotation=Quaternion.Euler(0,0,-wheelAngle)*transform.rotation;
		}

	bool IsBreaking(float vertInput,float enginePush)
		{
			return (vertInput > 0 && enginePush <0 || vertInput <0 && enginePush >0);
		}

	void HoldVehicle()
		{

			if(vehichleUnderHold)
				{
					if(Time.time - lastHoldClockTime < maxHoldTime)
						{
							pushFromEngine=0f;
							// Debug.Log("on hold");
							return;
						}
					
					vehichleUnderHold=false;
				}
			else
				{
					if(Mathf.Abs(pushFromEngine) < 0.1f && holdable)
						{
							vehichleUnderHold=true;
							holdable=false;
							lastHoldClockTime=Time.time;
						}
					else if(Mathf.Abs(pushFromEngine) > 0.1f)
						{
							if(!holdable) holdable=true;
						}
				}
		}
	void DoNormalAIMovement()
		{
			
		}
	void AIMovement()
		{
			/*
			if(!DetectObjectFront())
				SimulatedMovement(0f,1f);
			else
				SimulatedMovement(-1f,-0.3f);
			*/
			if(presentState==VehState.NORMAL)
				{
					SimulatedMovement(aiTireTiltControl,1f);
					StraightenVehicle();
					if(DetectObjectFront(3*raycastLengthFront))
						{
						presentState=VehState.SLOW;
						Debug.Log("State changed to slow");
						}
				}
			else if(presentState==VehState.SLOW)
				{

					SimulatedMovement(aiTireTiltControl,ApplyBreaks(0.2f));
					StraightenVehicle();
					if(DetectObjectFront(2*raycastLengthFront))
						{
							DecideTurnDirection();
							hungStateWheelTurn=(turnLeft?1f:-1f);
							hungState=VehState.TURN;
							presentState=VehState.STOPPED;
							Debug.Log("State changed to STOPPED");
						}
				}
			else if(presentState==VehState.TURN)
				{
					if(turnLeft)
						SimulatedMovement(1f,0.2f);
					else
						SimulatedMovement(-1f,0.2f);
					if(!DetectObjectFront(2*raycastLengthFront))
						{
							presentState=VehState.NORMAL;
							Debug.Log("State changed to normal");
						}

				}
			else if(presentState==VehState.STOPPED)
				{
					SimulatedMovement(hungStateWheelTurn,ApplyBreaks(0f));
					// Debug.Log(tyreTilt);
					if(Mathf.Abs(tyreTilt) >= 0.9f * maxTyreTilt)
						{
							presentState=hungState;
						}

				}

		}
float ApplyBreaks(float desiredSpeed)
	{
		if(rb.velocity.magnitude > desiredSpeed * speed)
			return -1f;
		else return desiredSpeed;
	}
void StraightenVehicle()
	{
		aiTireTiltControl=0f;return;
		if(frontLeftTyre.DetectObjectAndDistance(Vector2.left) < sepFromFence*0.8f)
			aiTireTiltControl=-0.6f;
		else if(frontLeftTyre.DetectObjectAndDistance(Vector2.left) > sepFromFence*1.2f)
			aiTireTiltControl=0.6f;
		else 
			aiTireTiltControl=0f;
	}
void DecideTurnDirection()
	{
		if(frontLeftTyre.DetectObjectAndDistance(Vector2.left) <  frontRightTyre.DetectObjectAndDistance(Vector2.right) )
			{
			Debug.Log("decided to turn left");
			turnLeft=true;
			}
		else
			{
			Debug.Log("decided to turn right");
			turnLeft=false;
			}
	}
//Raycast Functions


bool detectObjectsWithTag(Vector2 origin,Vector2 dir,string[] t_gs)
        {
            
            RaycastHit2D[] hits=Physics2D.RaycastAll(origin,dir.normalized,dir.magnitude);
            
            foreach(RaycastHit2D hitx in hits)
                {
                	foreach(string t_g in t_gs)
                    if(hitx.collider.tag==t_g)
                        return true;
                }
            return false;
        }
bool DetectObjectFront(float x=-1f)
		{
			
			float raycastLengthFront_local=raycastLengthFront;
			if(x!=-1f)
				raycastLengthFront_local=x;
			Debug.DrawRay(transform.position- transform.rotation*Vector2.right*sideWaysOffset,
				          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthFront_local,
				          Color.red);
			Debug.DrawRay(transform.position,
				          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthFront_local,
				          Color.red);
			Debug.DrawRay(transform.position+ transform.rotation*Vector2.right*sideWaysOffset,
				          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthFront_local,
				          Color.red);
			

			string[] tags=new string[]{"Fence"};

			bool hit1=detectObjectsWithTag(transform.position- transform.rotation*Vector2.right*sideWaysOffset,
							          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthFront_local,
							          tags);			
			bool hit2=detectObjectsWithTag(transform.position,
				          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthFront_local,
				          tags);			
			bool hit3=detectObjectsWithTag(transform.position+ transform.rotation*Vector2.right*sideWaysOffset,
				          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthFront_local,
				          tags);
			return hit1 || hit2 || hit3;
		}
//Raycast Functions
	void SimulatedMovement(float horizInput,float vertInput)
		{		
			
			if(Mathf.Abs(vertInput) > 0f)
				{
				  if(Mathf.Abs(pushFromEngine) < frictionFactor && !IsBreaking(vertInput,pushFromEngine))
				  	pushFromEngine=Mathf.Lerp(pushFromEngine,vertInput,Time.deltaTime * accVal*1f);
				  else
					pushFromEngine=Mathf.Lerp(pushFromEngine,vertInput,Time.deltaTime * accVal);

				  HoldVehicle();
				}
			else
				{
				pushFromEngine=Mathf.Lerp(pushFromEngine,vertInput,Time.deltaTime * slowDeccVal);
				}


			//Tilting tyre with Mathf.Lerp starts
			
			if(horizInput > 0f)
				{
					tyreTilt=Mathf.Lerp(tyreTilt,maxTyreTilt,steeringSpeed * k_push/pushFromEngine * Time.deltaTime);					
				}
			else if(horizInput < 0f)
					tyreTilt=Mathf.Lerp(tyreTilt,-maxTyreTilt,steeringSpeed * k_push/pushFromEngine * Time.deltaTime);	
			else	
					tyreTilt=Mathf.Lerp(tyreTilt,0f,20f*steeringSpeed * Time.deltaTime);					
			tyreScript1.TurnTyres(tyreTilt);
			tyreScript2.TurnTyres(tyreTilt);
			//Tilting tyre with Mathf.Lerp ends


			float tiltAdjutmentLocal=tyreTilt*pushFromEngine*tyreTiltAdjFraction;			
			// float peWeightageLocal=(pushFromEngine==0f)?0f:1f;
			// float tiltAdjutmentLocal=peWeightage*pushFromEngine+ttWeightage * tyreTilt * peWeightageLocal;

			if(pushFromEngine > 0f)
				transform.rotation=Quaternion.Euler(0,0,tiltAdjutmentLocal)*transform.rotation;
			else if(pushFromEngine < 0f)
				transform.rotation=Quaternion.Euler(0,0,tiltAdjutmentLocal)*transform.rotation;
			rb.velocity=Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*speed*pushFromEngine;
			//pushFromEngine and tyreTilt may be appropriately weighted if need be

		}
}

