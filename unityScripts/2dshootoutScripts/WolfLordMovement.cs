using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WolfLordMovement : MonoBehaviour 
	{

	Rigidbody2D rb;
	public GameObject target;
	public GameObject torso;
	public float wolfSpeed=5f,targetProximDistance=3f;
	WolfAnimationControl tScript;
	public float directVisibilityProximDistance=100f;
	public GameObject fireBallx,cpoof;
	public float maxTimeSinceLastAttack=10f;
	float attackTimer=0f;
	string[] tags=new string[]{"bush","gift","pipe","bfire"};
	string[] tags_player=new string[]{"Player"};
	public float raycastLengthFront=10f,sidewayOffset=2f,playerProximFront=4f;
	public float maxHealth;
	float health;

	void Start () 
		{
		health=maxHealth;
		rb=GetComponent<Rigidbody2D>();
		tScript= torso.GetComponent<WolfAnimationControl>();
		// EmitFireballs();
		}
	public void TakeDamage(float dmg)
		{
			health-=dmg;
						
			if(health>=0)
				GameObject.Find("WolfBar").GetComponent<Image>().fillAmount=health/maxHealth;
			
			if(health < 0)
				{
					Instantiate(cpoof,transform.position,Quaternion.identity);
					Destroy(gameObject);
				}
		}
	void EmitFireballs()
		{

			for(int i=0;i < 6;i++)
				{
					GameObject g=Instantiate(fireBallx,transform.position,Quaternion.identity);
					g.SendMessage("getOutwardAngle",i*45f);
				}
		}
	public void InflictDamage()
		{
		if(DetectPlayerInDirection(target.transform.position-transform.position))
			target.GetComponent<PlayerMovement>().TakeDamage(20f);
		}



		//  RAYCAST FUNCTIONS
	bool DetectPlayerInDirection(Vector2 dir)
		{
			// Debug.DrawRay(transform.position-Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
			// 	          dir.normalized*playerProximFront,
			// 	          Color.red);
			// Debug.DrawRay(transform.position,
			// 	          dir.normalized*playerProximFront,
			// 	          Color.red);
			// Debug.DrawRay(transform.position+Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
			// 	          dir.normalized*playerProximFront,
			// 	          Color.red);
			
			bool hit1=detectObjectWithTag(transform.position-Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
							          dir.normalized*playerProximFront,
							          tags_player);			
			bool hit2=detectObjectWithTag(transform.position,
				          dir.normalized*playerProximFront,
				          tags_player);			
			bool hit3=detectObjectWithTag(transform.position+Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
				          dir.normalized*playerProximFront,
				          tags_player);
			return hit1 || hit2 || hit3;
		}
    bool DetectFenceInDirection(Vector2 dir)
		{
			// Debug.DrawRay(transform.position-Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
			// 	          dir.normalized*raycastLengthFront,
			// 	          Color.red);
			// Debug.DrawRay(transform.position,
			// 	          dir.normalized*raycastLengthFront,
			// 	          Color.red);
			// Debug.DrawRay(transform.position+Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
			// 	          dir.normalized*raycastLengthFront,
			// 	          Color.red);
			
			bool hit1=detectObjectWithTag(transform.position-Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
							          dir.normalized*raycastLengthFront,
							          tags);			
			bool hit2=detectObjectWithTag(transform.position,
				          dir.normalized*raycastLengthFront,
				          tags);			
			bool hit3=detectObjectWithTag(transform.position+Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
				          dir.normalized*raycastLengthFront,
				          tags);
			return hit1 || hit2 || hit3;
		}

		//dd
    bool detectDirectVisibility(Vector2 origin,Vector2 dir,string t_g)
    	{
    		// Debug.DrawRay(origin,
				  //         dir.normalized*18f,
				  //         Color.red);
    		 RaycastHit2D[] hits=Physics2D.RaycastAll(origin,dir.normalized,directVisibilityProximDistance);
            float minDist=26000f,minDistTag=26001f;
            foreach(RaycastHit2D hitx in hits)
                {
                	if(hitx.collider.tag=="wolflord") continue;
                	if(minDist > hitx.distance) minDist=hitx.distance;
                    if(hitx.collider.tag==t_g)
                        if(minDistTag > hitx.distance) minDistTag=hitx.distance;
                }        
            if(minDistTag <= minDist)
            	{            	
            	return true;
            	}
           
            return false;
    	}

    bool detectObjectWithTag(Vector2 origin,Vector2 dir,string[] t_gs)
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
		//dd

		//  *******************************************************************************************
	void CheckIfTooLongSinceLastAttack()
		{
			attackTimer+=Time.deltaTime;
			if(attackTimer > maxTimeSinceLastAttack)
				{
					attackTimer=0f;
					
					if(Vector2.Distance(transform.position,target.transform.position)>25f)
					tScript.DisappearNTeleport();
					else
					EmitFireballs();

				}
		}
	public void TeleportNearTarget()
		{
			Debug.Log("called");
			Vector3 dir=transform.position-target.transform.position;
			float delta=dir.magnitude/5;
			Vector3 pos=Vector3.zero;
			bool found=false;
			for(int i=2;i <5;i++ )
				{
					pos=target.transform.position+delta*i*dir.normalized;
					if(!Physics2D.OverlapCircle(pos,1f))
						{
						found=true;
						break;
						}
				}
			transform.position=pos;
		}
	void Update () 
		{


		if(!target)
			return;
		CheckIfTooLongSinceLastAttack();
		bool hindrance=DetectFenceInDirection(target.transform.position-transform.position);

		if(hindrance)
			StrafeAroundHindrance(target.transform.position);
		else
			TurntowardsTarget(target.transform.position);
		MoveForward();
		}

	void StrafeAroundHindrance(Vector2 tgt)
    	{

    		transform.rotation=Quaternion.Slerp(transform.rotation,
											    getRotationAngle(transform.position,tgt),
											    10f*Time.deltaTime
				         					   );
    	}

	void TurntowardsTarget(Vector2 tgt)
    	{

    		transform.rotation=Quaternion.Slerp(transform.rotation,
											    Quaternion.Euler(0,0,-90)*getRotationAngle(transform.position,tgt),
											    10f*Time.deltaTime
				         					   );
    	}
   	Quaternion getRotationAngle(Vector3 src,Vector3 tgt)
        {
            float angle=Mathf.Atan2((tgt.y-src.y),(tgt.x-src.x)) * Mathf.Rad2Deg;           
            return Quaternion.Euler(0,0,angle);
        }
    void MoveForward()
    	{
    		if(Vector2.Distance(transform.position,target.transform.position) < targetProximDistance)
    			{
    			rb.velocity=Vector2.zero;
    			attackTimer=0f;
    			tScript.Attack();
    			}
    		else
    			rb.velocity=Quaternion.Euler(0,0,90) * transform.rotation * Vector2.right * wolfSpeed;
    		tScript.Move(rb.velocity);
    	}




	}
