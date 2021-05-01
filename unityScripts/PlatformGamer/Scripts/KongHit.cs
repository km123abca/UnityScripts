using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongHit : MonoBehaviour 
	{

	public float force=100f;
	int dir;
	void Start () 
		{
			
		}	
	
	void Update () 
		{
		
		}
	void OnCollisionEnter2D(Collision2D col)
		{
			dir=transform.parent.GetComponent<KongScript>().Dir;
			if(col.gameObject.tag=="Player")
				{
					col.gameObject.GetComponent<Rigidbody2D>().AddForce(dir*Vector2.right * force);
					col.gameObject.GetComponent<MannyScript>().TakeDamage(10f);
				}
		}

	}
