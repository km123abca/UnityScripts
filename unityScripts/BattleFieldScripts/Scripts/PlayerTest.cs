using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour 
	{
	public float verticalOffset=3f,raycastLengthFront=5f,longRayDist=50f;
	string enemyString="Bug";
	int debugCounter=0;
	void Start () 
		{
			Quaternion dir=FindExit();
		}	
	
	void Update () 
		{
		
		}

Quaternion FindExit()
		{
			
			float angleToCenter=ClearPathToCenter();
			if(angleToCenter!=0)
				{
					// Displayx("CLear path exists");
					return Quaternion.Euler(0,0,angleToCenter);
				}

			for(float i=0f;i < 360f;i+=1f)
				{
					if(!DetectObjectFront(i))
						return Quaternion.Euler(0,0,i);
				}
			return Quaternion.Euler(0,0,270);//kmhere this needs review
		}

float ClearPathToCenter()
	{
		GameObject center=GameObject.Find("CenterPoint");
		Displayx(center.transform.position.x+","+center.transform.position.y);
		string[] tagsx=new string[]{"Bug"};
		if ( DetectWallOnAnyDir(getRotationAngle_angle(transform.position,center.transform.position),tagsx) )
			return 0f;
		else
			return getRotationAngle_angle(transform.position,center.transform.position);
		  
	}

bool DetectWallOnAnyDir(float checkAngle,string[] tagsrec)
		{

			// string[] tagsx=new string[]{"Pipe"};
			
			bool hit1=detectObjectsWithTag(transform.position+Quaternion.Euler(0,0,90)*
								 Quaternion.Euler(0,0,checkAngle)*
								 Vector2.right*verticalOffset,
							          Quaternion.Euler(0,0,checkAngle)*Vector2.right*longRayDist,
							          tagsrec);
			bool hit2=detectObjectsWithTag(transform.position,
					                  Quaternion.Euler(0,0,checkAngle)*Vector2.right*longRayDist,
							          tagsrec);
			bool hit3=detectObjectsWithTag(transform.position-Quaternion.Euler(0,0,90)*
								 Quaternion.Euler(0,0,checkAngle)*
								 Vector2.right*verticalOffset,
							          Quaternion.Euler(0,0,checkAngle)*Vector2.right*longRayDist,
							          tagsrec);
			// Debug.DrawRay(transform.position,
			// 	          Quaternion.Euler(0,0,checkAngle)*Vector2.right*longRayDist,
			// 	          Color.red);	

			
			return hit1||hit2||hit3 ;
		}

Quaternion getRotationAngle(Vector3 src,Vector3 tgt)
		{
			float angle=Mathf.Atan2((tgt.y-src.y),(tgt.x-src.x)) * Mathf.Rad2Deg;			
			return Quaternion.Euler(0,0,angle);
		}
float getRotationAngle_angle(Vector3 src,Vector3 tgt)
		{
			float angle=Mathf.Atan2((tgt.y-src.y),(tgt.x-src.x)) * Mathf.Rad2Deg;			
			return angle;
		}

bool DetectObjectFront(float angleOffset=0f)
		{
			Quaternion aoq=Quaternion.Euler(0,0,angleOffset);
			Debug.DrawRay(transform.position-Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*verticalOffset,
				          aoq*transform.rotation*Vector2.right*raycastLengthFront,
				          Color.red);
			Debug.DrawRay(transform.position,
				          aoq*transform.rotation*Vector2.right*raycastLengthFront,
				          Color.red);
			Debug.DrawRay(transform.position+Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*verticalOffset,
				          aoq*transform.rotation*Vector2.right*raycastLengthFront,
				          Color.red);
			

			string[] tags=new string[]{"Pipe",enemyString};

			bool hit1=detectObjectsWithTag(transform.position-Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*verticalOffset,
							          aoq*transform.rotation*Vector2.right*raycastLengthFront,
							          tags);			
			bool hit2=detectObjectsWithTag(transform.position,
				          aoq*transform.rotation*Vector2.right*raycastLengthFront,
				          tags);			
			bool hit3=detectObjectsWithTag(transform.position+Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*verticalOffset,
				          aoq*transform.rotation*Vector2.right*raycastLengthFront,
				          tags);
			return hit1 || hit2 || hit3;
		}
	void Displayx(string x)
		{
			debugCounter+=1;
			Debug.Log(debugCounter+ ": "+x);
		}

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

	}
