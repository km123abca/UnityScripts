using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeFire : MonoBehaviour 
	{
	Rigidbody2D rb;
	public float bulletSpeed=10f;
	int dirb;
	void Start () 
		{
		rb=GetComponent<Rigidbody2D>();
		rb.velocity=dirb * Vector2.left * bulletSpeed;
		}
	
	void GetBulletDirection(int dir)
		{
			dirb=dir;
		}
	void Update () 
		{
		
		}
	void OnCollisionEnter2D(Collision2D col)
		{
			if(col.gameObject.tag=="Player")
				{
					col.gameObject.GetComponent<MannyScript>().TakeDamage(10f);
					Destroy(gameObject);
				}
			// else if(col.gameObject.tag!="bee")
			// 	Destroy(gameObject);
		}
	// void OnBecameInvisible()
	// 	{
	// 		Destroy(gameObject);
	// 	}
	}
