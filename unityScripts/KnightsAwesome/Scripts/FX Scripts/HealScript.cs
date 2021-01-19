using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealScript : MonoBehaviour {

	public float healAmount=20f;
	void Start () 
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthScript>().HealPlayer(healAmount);
		// Debug.Log("player health increased to "+GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthScript>().health);
	}
	
	
}
