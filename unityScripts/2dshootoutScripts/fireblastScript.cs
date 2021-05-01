using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireblastScript : MonoBehaviour 
	{
	float expulsionAngle;
	public float rotSpeed=10f;
	public float fireBlastSpeed=10f;
	Rigidbody2D rb;
	void Start () 
		{
			rb=GetComponent<Rigidbody2D>();
			rb.velocity=Quaternion.Euler(0,0,expulsionAngle) * Vector2.right * fireBlastSpeed;
		}	
	
	void Update () 
		{

			transform.rotation=Quaternion.Euler(0,0,rotSpeed) * transform.rotation;
		}
	void OnBecameInvisible()
		{
			Destroy(gameObject);
		}
	void getOutwardAngle(float ang)
		{
			expulsionAngle=ang;
		}
	void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.tag != "fireblast" && col.gameObject.tag != "wolflord" && col.gameObject.tag != "Player" 
				&& col.gameObject.tag != "bfire" && col.gameObject.tag != "pipe"
				)
				{
					Destroy(col.gameObject);
					Destroy(gameObject);
				}
			else if(col.gameObject.tag=="Player")
				{
					col.gameObject.GetComponent<PlayerMovement>().TakeDamage(50f);
					Destroy(gameObject);
				}
			else if(col.gameObject.tag == "bfire" || col.gameObject.tag == "pipe")
				{
					Destroy(gameObject);
				}
		}

	}
