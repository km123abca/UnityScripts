using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailMovement : MonoBehaviour 
	{
	Rigidbody2D rb;
	public float snailSpeed=5f;
	int count=0;
	int dir=1;
	
	void Start () 
		{
		 rb=GetComponent<Rigidbody2D>();
		 rb.velocity= Vector2.left * snailSpeed;

		}
	
	void Update () 
		{
			
			rb.velocity=dir * Vector2.left* snailSpeed;

		}
	public void DestroySelf()
		{
			Destroy(gameObject);
		}
	void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.tag == "stopPoint")
				{
					FlipSnail();
					dir*=-1;
				}
		}
	void FlipSnail()
		{
			Vector3 scale= transform.localScale;			
			scale.x=scale.x*-1;
			transform.localScale= scale;
		}



	}
