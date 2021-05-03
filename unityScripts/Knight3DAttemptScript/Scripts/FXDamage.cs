using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXDamage : MonoBehaviour 
	{
	LayerMask enemyLayer;
	public float radius =0.5f;
	public float damageCount=10f;
	PlayerHealthAI enemyScript;
	bool collided;
	
	void Start () 	
		{
		
		}
	
	void GetEnemyLayer(LayerMask el)
		{
			enemyLayer=el;
		}
	void Update () 
		{
			Collider[] hits=Physics.OverlapSphere(transform.position,radius,enemyLayer);//
			foreach(Collider c  in hits)
				{
				if(c.isTrigger)	continue;
				// enemyScript=c.gameObject.GetComponent<EnemyMovementAI>();
				enemyScript=c.gameObject.GetComponent<PlayerHealthAI>();
				collided=true;
				}
			if(collided)
				{					
					enemyScript.TakeDamage(damageCount);
					enabled=false;
				}
		}
		
	}
