using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public float health =100f;
	public void TakeDamage(float damageAmount)	
		{
			health -=damageAmount;
			Debug.Log("Player says alright alright:"+health);
			if(health <= 0)
				{
					//kill the player 
				}
		}
	public void Update()
		{
			// Debug.Log("Player health is activ");
		}
}
