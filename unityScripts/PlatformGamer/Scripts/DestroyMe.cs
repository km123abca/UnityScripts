using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour {

	public float destroyAfterTime=4f;
	// public GameObject cloudPoof;
	
	void Start () {

		
		Destroy(gameObject,destroyAfterTime);
		
	}
	
	

}
