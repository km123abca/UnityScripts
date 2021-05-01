using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour 
	{
	public float sidewayOffset=2f;
	public float raycastLengthFront=10f;
	public GameObject tBall;
	public GameObject tBallPosition;
	public float maxBallInterval=3f;
	float ballIntervalTimer=0f;

	void Start () 
		{
		  ballIntervalTimer=maxBallInterval;
		}
	
	
	void Update () 
		{
		 float x=DetectPlayerInDirection(Vector2.left,raycastLengthFront,"Player");
		 if(x < 9)
		 	{
		 		if(ballIntervalTimer >= maxBallInterval)
		 			{
		 				SpawnBall();
		 				ballIntervalTimer=0f;
		 			}
		 		else
		 			{
		 				ballIntervalTimer=ballIntervalTimer+  Time.deltaTime;
		 			}
		 	}
		 // else
		 // 	{
		 // 		ballIntervalTimer=maxBallInterval;
		 // 	}
		}

	void SpawnBall()
		{
			GameObject g=Instantiate(tBall,tBallPosition.transform.position,Quaternion.identity);
			// g.SendMessage(Vector2.left)
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
