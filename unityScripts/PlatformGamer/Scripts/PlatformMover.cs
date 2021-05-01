using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour 
	{

	Rigidbody2D rb;
	public bool isMovementHorizontal,isLeft;
	Vector2 dirOfMovement;
	public float speed=2f;
	bool moving,playerOnTop,startDirReversal;
	void Start () 
		{
		  Vector2 chosenDir=Vector2.right;
		  if(isLeft) chosenDir=Vector2.left;
		  if(isMovementHorizontal)
		  	dirOfMovement=chosenDir;
		  else
		  	dirOfMovement=Vector2.up;
		  rb=GetComponent<Rigidbody2D>();	  
		  
		}
	
	
	void Update () 
		{
			if(moving && rb.velocity==Vector2.zero) rb.velocity= dirOfMovement * speed;
			else if(!moving && rb.velocity!=Vector2.zero) rb.velocity=Vector2.zero;			
		}

	public Vector2 GetVel()
		{
			return rb.velocity;
		}

	void OnCollisionEnter2D(Collision2D col)
		{

			if(col.gameObject.tag=="Player")
				{
					// Debug.Log("parent set");
					moving=true;
					playerOnTop=true;
					col.collider.transform.SetParent(transform);
				}
		}
	void OnCollisionExit2D(Collision2D col)
		{
			if(col.gameObject.tag=="Player")
				{					
					playerOnTop=false;
					col.collider.transform.SetParent(null);
				}
		}

	void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.tag=="platformTouchPoint")
				{
					if(!playerOnTop)
						moving=false;
					if(startDirReversal)
						{
						dirOfMovement*=-1;
						rb.velocity= dirOfMovement * speed;
						}
					
				}
		}

	void OnTriggerExit2D(Collider2D col)
		{
			if(col.gameObject.tag=="platformTouchPoint")
				{
					if(!startDirReversal) startDirReversal=true;
				}
		}

	}
