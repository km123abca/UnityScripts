using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour {

	GameObject parentBug;
	public float laserDamage=100f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GetParent(GameObject go)
		{
			parentBug=go;
		}

	void OnTriggerEnter2D(Collider2D col)
		{
			// if(col.gameObject.tag=="Pipe")
			// 	Destroy(gameObject);
			if(col.gameObject.tag==parentBug.GetComponent<BugWrapper>().enemyString)
				{
					if(col.gameObject.GetComponent<WrapperControl>().IsConspicous)
						{
						if(col.gameObject.GetComponent<WrapperControl>().IsAI)
							col.gameObject.GetComponent<WrapperControl>().TakeDamageAI(laserDamage,false);
						else
							col.gameObject.GetComponent<WrapperControl>().TakeDamage(laserDamage,false);
						// Destroy(gameObject);
						}
				}
		}

}
