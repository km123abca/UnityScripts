using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTyreBehav : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void TurnTyres(float angle)
		{
			transform.localRotation=Quaternion.Euler(0,0,angle);
		}
}
