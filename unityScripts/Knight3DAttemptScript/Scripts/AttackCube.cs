using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCube : MonoBehaviour {

	
	void Start () 
		{
		
		}
	
	
	void Update () 
		{
		
		}

	void OnTriggerEnter(Collider col)		
		{
			if(col.gameObject.tag=="Player")
				{
					col.gameObject.GetComponent<PlayerHealth>().TakeDamage(10f);
				}
		}
}
