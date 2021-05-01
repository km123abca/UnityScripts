using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour 
	{

	Rigidbody2D rb;
	float botSpeed=5f;
	bool movingUp=true;
	void Start () 
		{
		  rb=GetComponent<Rigidbody2D>();
		  rb.velocity=Vector2.up*botSpeed;
		}
	
	
	void Update () 
		{
		
		}
	void OnCollisionEnter2D(Collision2D col)
		{
			Debug.Log("collision");
			if(col.gameObject.tag=="StopPoint")
				{
					if(movingUp)
						{
						 rb.velocity=Vector2.down*botSpeed;
						 movingUp=false;
						}
					else 
						{
						 rb.velocity=Vector2.up*botSpeed;
						 movingUp=true;
						}
				}
		}

	}
