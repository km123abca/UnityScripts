using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float health = 100f;
	public void TakeDamage(float damageAmount)
		{	
			health-=damageAmount;
			Debug.Log("Damage recieved:"+health);
			if(health <= 0)
				{
					//Destroy the EnemyHealth 
				}
		}
}
