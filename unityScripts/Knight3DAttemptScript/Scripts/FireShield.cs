using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShield : MonoBehaviour {

	GameObject parent;
	PlayerHealthAI pscript;
	void Start () 
		{
		 pscript=parent.GetComponent<PlayerHealthAI>();
		 pscript.shielded=true;	
		 Debug.Log("shield enabled");	
		}
	
	void Update () 
		{
		
		}
	void GetParent(GameObject parentReceived)
		{
			parent=parentReceived;
			Debug.Log(parent.tag+" received");
		}


	void OnEnable () 
		{
			// pscript=parent.GetComponent<PlayerHealthAI>();

			// transform.parent.GetComponent<PlayerHealthAI>().shielded=true;
		}
	void OnDisable () 
		{
			// transform.parent.GetComponent<PlayerHealthAI>().shielded=false;
			pscript.shielded=false;
			Debug.Log("shield disabled");
		}
}
