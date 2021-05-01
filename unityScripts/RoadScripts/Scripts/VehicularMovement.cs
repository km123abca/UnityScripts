using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicularMovement : MonoBehaviour {

	Rigidbody2D rb;
	public float speed=5f;

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

	// float rotationSpeed;
	void Start () {
		rb=GetComponent<Rigidbody2D>();
		
		
	}
	
	
	void Update () 
		{
			float horizInput=-Input.GetAxis("Horizontal");
			float vertInput = Input.GetAxis("Vertical");
			SimulatedMovement(horizInput,vertInput);
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

	void SimulatedMovement(float horizInput,float vertInput)
		{		
			
			if(Mathf.Abs(vertInput) > 0f)
				{
				  if(Mathf.Abs(pushFromEngine) < frictionFactor)
				  	pushFromEngine=Mathf.Lerp(pushFromEngine,vertInput,Time.deltaTime * accVal*0.2f);
				  else
					pushFromEngine=Mathf.Lerp(pushFromEngine,vertInput,Time.deltaTime * accVal);
				}
			else
				{
				pushFromEngine=Mathf.Lerp(pushFromEngine,vertInput,Time.deltaTime * slowDeccVal);
				}
			//Adjusting Tyre Tilt as per horizontal input starts
				/*
			if(Mathf.Abs(horizInput) > 0f)
				{
					tyreTilt+=horizInput * tyreTiltSpeed;
					tyreTilt=Mathf.Clamp(tyreTilt,-maxTyreTilt,maxTyreTilt);
				}
			else
				{
					if(tyreTilt > 0f)
						tyreTilt-=fastReturnTyreTiltSpeed;
					else if(tyreTilt < 0f)
						tyreTilt+=fastReturnTyreTiltSpeed;
					if(Mathf.Abs(tyreTilt) <= fastReturnTyreTiltSpeed)
						tyreTilt=0f;
				}
			tyreScript1.TurnTyres(tyreTilt);
			tyreScript2.TurnTyres(tyreTilt);
			*/
			//Adjusting Tyre Tilt as per horizontal input ends

			//Tilting tyre with Mathf.Lerp starts
			
			if(horizInput > 0f)
				{
					tyreTilt=Mathf.Lerp(tyreTilt,maxTyreTilt,steeringSpeed * Time.deltaTime);					
				}
			else if(horizInput < 0f)
					tyreTilt=Mathf.Lerp(tyreTilt,-maxTyreTilt,steeringSpeed * Time.deltaTime);	
			else	
					tyreTilt=Mathf.Lerp(tyreTilt,0f,20f*steeringSpeed * Time.deltaTime);					
			tyreScript1.TurnTyres(tyreTilt);
			tyreScript2.TurnTyres(tyreTilt);			

			

			//Tilting tyre with Mathf.Lerp ends

			//This is too rigorous and probably inaccurate starts
			/*
			float deltaCirc=Mathf.Cos(tyreTilt/180* Mathf.PI)*speed*pushFromEngine*0.01f;
			float angleToRotate=(deltaCirc/tyreSeperation) /Mathf.PI * 180;
			transform.rotation= Quaternion.Euler(0,0,angleToRotate) * transform.rotation;
			*/
			//This is too rigorous and probably inaccurate ends

			if(pushFromEngine > 0f)
				transform.rotation=Quaternion.Euler(0,0,tyreTilt*pushFromEngine*tyreTiltAdjFraction)*transform.rotation;
			else if(pushFromEngine < 0f)
				transform.rotation=Quaternion.Euler(0,0,tyreTilt*pushFromEngine*tyreTiltAdjFraction)*transform.rotation;
			rb.velocity=Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*speed*pushFromEngine;

		}
}

