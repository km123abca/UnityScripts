using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
	{

	Rigidbody2D rb;
	Quaternion qt;
	public float bulletSpeed=10f;
	GameObject parentBug;

	void Start () 
		{
		  rb=GetComponent<Rigidbody2D>();	
		  rb.velocity= Quaternion.Euler(0,0,90)*qt * Vector2.right * bulletSpeed;
		}
	
	
	void Update () 
		{
			
		}

	void GetRotation(Quaternion qt_x)
		{
			qt=qt_x;
		}
	void GetParentBug(GameObject pb)
		{
			parentBug=pb;
		}

	void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.tag=="Pipe")
				Destroy(gameObject);
			if(col.gameObject.tag==parentBug.GetComponent<BugWrapper>().enemyString)
				{
					if(col.gameObject.GetComponent<WrapperControl>().IsConspicous)
						{
						if(col.gameObject.GetComponent<WrapperControl>().IsAI)
							col.gameObject.GetComponent<WrapperControl>().TakeDamageAI(5f,false);
						else
							col.gameObject.GetComponent<WrapperControl>().TakeDamage(5f,false);
						Destroy(gameObject);
						}
				}
		}

	}
