using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDXdamage : MonoBehaviour {

	public LayerMask playerLayer;
	public float radius=0.5f;
	public float damageCount=10f;
	PlayerHealthScript playerHealth;
	bool collided;
	void Start () 
		{
			
		}
	
	
	void Update () 
		{
			Collider[] hits=Physics.OverlapSphere(transform.position,radius,playerLayer);
			foreach(Collider c in hits)
				{
					playerHealth=c.GetComponent<PlayerHealthScript>();
					collided=true;
				}
			if(collided)
				{
					playerHealth.TakeDamage(damageCount);
					enabled=false;
				}
		}
}
