using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAndShowPoof : MonoBehaviour 
	{

    public GameObject cpoof;
    public float afterTime=3f;
    float timer=0f;

	void Start () 
		{
			Destroy(gameObject,afterTime);
		}
	
	
	void Update () 	
		{
			timer+= Time.deltaTime;
			if(timer >= (afterTime-Time.deltaTime))
				{
					Instantiate(cpoof,transform.position,Quaternion.identity);
				}

		}

	void OnCollisionEnter2D(Collision2D col)
		{
			if(col.gameObject.tag=="Player")
				{
					col.gameObject.GetComponent<MannyScript>().TakeDamage(5f);
					Instantiate(cpoof,transform.position,Quaternion.identity);
					Destroy(gameObject);
				}
		}


	}
