using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour 
	{

	float expulsionAngle;
	public float rotSpeed=10f;
	public float fireBlastSpeed=10f;
	Rigidbody2D rb;
	GameObject parentBug;

	void Start () 
		{
			rb=GetComponent<Rigidbody2D>();
			rb.velocity=Quaternion.Euler(0,0,expulsionAngle) * Vector2.right * fireBlastSpeed;
		}
	void GetOutwardAngle(float ang)
		{
			expulsionAngle=ang;
		}	
	void GetParent(GameObject go)
		{
			parentBug=go;
		}
	
	void Update () 
		{
		transform.rotation=Quaternion.Euler(0,0,rotSpeed) * transform.rotation;
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
