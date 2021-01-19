using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShield : MonoBehaviour {

	PlayerHealthScript playerHealth;
	void Awake () 
		{
		 playerHealth=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthScript>();
		
		}
	
	
	void OnEnable () 
		{
			playerHealth.Shielded=true;
			


		}
	void OnDisable () 
		{
			playerHealth.Shielded=false;
		}
}
