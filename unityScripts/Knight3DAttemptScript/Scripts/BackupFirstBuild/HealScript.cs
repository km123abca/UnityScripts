using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealScript : MonoBehaviour {

	public float healAmount=20f;
	void Start () 
	{
		try
		{
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().HealPlayer(healAmount);
		}
		catch
		{
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthAI>().HealPlayer(healAmount);
		}
		
	}
	
	
}