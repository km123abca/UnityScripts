using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeMovement : MonoBehaviour 
	{

	Rigidbody2D rb;
	public float sidewayOffset=1f,raycastLengthFront=15f;
	int dirx=1;
	public float bulletFireRayCast=13f,approachRayCast=8f;
	public float maxTimeGapBeforeFire=2f;
	float timeGapBeforeFire;
	public GameObject flame;
	public float beeSpeed=2f;
	Vector2 initPos;
	public float initPositionProximity=0.2f;
	public GameObject target;
	void Start () 
		{
			// Instantiate(flame,transform.position,Quaternion.identity);
			timeGapBeforeFire=maxTimeGapBeforeFire;
			rb=GetComponent<Rigidbody2D>();
			initPos=transform.position;
			// Debug.Log("initial x position:"+(initPos.x).ToString());
		}
	
	void GetBack()
		{
			// Debug.Log("xpos:"+transform.position.x);
			// Debug.Log("initial Position:"+initPos.x);
			// GameObject.Find("DebugText").GetComponent<Text>().text=(transform.position.x).ToString();

			/*
			if( (dirx > 0 && transform.position.x >= initPos.x) || (dirx < 0 && transform.position.x <= initPos.x) )
				{
					transform.position=initPos;
					rb.velocity=Vector2.zero;
				}
			else
			*/
			if(Mathf.Abs(transform.position.x-initPos.x) < initPositionProximity)
				{
				transform.position=initPos;
				rb.velocity=Vector2.zero;
				}
				if(initPos.x < transform.position.x)
					rb.velocity=Vector2.left * beeSpeed;
				else if(initPos.x > transform.position.x)
					rb.velocity=Vector2.right * beeSpeed;


		}
	void Pursue()
		{
			rb.velocity=dirx* Vector2.left * beeSpeed;
		}
	public void DestroySelf()
		{
			Destroy(gameObject);
		}
	void Update () 
		{

			if(target)
			{
			if ( (target.transform.position.x < transform.position.x && dirx!=1) ||
				 (target.transform.position.x > transform.position.x && dirx!=-1) 
			   )
				{
					Debug.Log("Flipped");
					dirx*=-1;
					FlipAccordingly(dirx);
				}
			}
			float x=DetectPlayerInDirection(dirx*Vector2.left,raycastLengthFront,"Player");
			// Debug.Log("distance:"+x);
			if(x <= approachRayCast)
				{
				  // Debug.Log("approaching");
					Pursue();
				}
			else if(x <= bulletFireRayCast)
				{
					GetBack();
					if(timeGapBeforeFire >= maxTimeGapBeforeFire)
						{
							Fire();
							timeGapBeforeFire=0f;
						}
					else
						{
							timeGapBeforeFire+=Time.deltaTime;
						}
				}
			else
				{
				GetBack();
			  	timeGapBeforeFire=maxTimeGapBeforeFire;
			  	}

		}

	void FlipAccordingly(int dir)
    	{
    		Vector2 sc=transform.localScale;
    		if(dir > 0)
    			{
    				sc.x= Mathf.Abs(sc.x);
    			}
    		else
    				sc.x= -Mathf.Abs(sc.x);
    		transform.localScale=sc;
    	}

	public void Fire()
		{
			Debug.Log("fired");
			GameObject go=Instantiate(flame,transform.position,Quaternion.identity);
			go.SendMessage("GetBulletDirection",dirx);
		}

    float detectObjectWithTag(Vector2 origin,Vector2 dir,string t_g)
        {
            
            RaycastHit2D[] hits=Physics2D.RaycastAll(origin,dir.normalized,dir.magnitude);
            
            foreach(RaycastHit2D hitx in hits)
                {                	
                    if(hitx.collider.tag==t_g)
                        return hitx.distance;
                }
            return -1;
        }

    float DetectPlayerInDirection(Vector2 dir,float raycastLengthFront,string tag)
		{
			Debug.DrawRay(transform.position-Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
				          dir.normalized*raycastLengthFront,
				          Color.red);
			Debug.DrawRay(transform.position,
				          dir.normalized*raycastLengthFront,
				          Color.red);
			Debug.DrawRay(transform.position+Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
				          dir.normalized*raycastLengthFront,
				          Color.red);
			
			float hit1=detectObjectWithTag(transform.position-Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
							          	  dir.normalized*raycastLengthFront,
							          	  tag);			
			float hit2=detectObjectWithTag(transform.position,
				          				  dir.normalized*raycastLengthFront,
				          				  tag);			
			float hit3=detectObjectWithTag(transform.position+Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
				          				  dir.normalized*raycastLengthFront,
				          				  tag);
			if(hit1!=-1)
				return hit1;
			else if(hit2!=-1)
				return hit2;
			else if(hit3!=-1)
				return hit3;

			return 99999f;
		}
	}
