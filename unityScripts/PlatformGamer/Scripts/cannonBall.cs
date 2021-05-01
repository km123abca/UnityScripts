using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonBall : MonoBehaviour 
	{
	Rigidbody2D rb;
	public float ballSpeed=5f;
	void Start () 	
		{
			rb=GetComponent<Rigidbody2D>();
		}
	
	
	void Update () 
		{
		  rb.velocity=Vector2.left * ballSpeed;
		}

	}
