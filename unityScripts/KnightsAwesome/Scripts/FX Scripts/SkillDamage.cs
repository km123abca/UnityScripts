using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : MonoBehaviour 
	{

	public LayerMask enemyLayer;
	public float radius =0.5f;
	public float damageCount=10f;
	private EnemyHealth enemyHealth;
	private bool collided;
	void Start () 
		{
		 
		}
	
	
	void Update () 
		{ 
			
			Collider[] hits=Physics.OverlapSphere(transform.position,radius,enemyLayer);
			// Debug.Log("Reporting "+hits.Length+" hits");
			foreach(Collider c  in hits)
				{
					if(c.isTrigger)
						continue;
					enemyHealth=c.gameObject.GetComponent<EnemyHealth>();
					collided=true;
				}
			if(collided)
				{
					
					enemyHealth.TakeDamage(damageCount);
					enabled=false;
				}
		}
	}
