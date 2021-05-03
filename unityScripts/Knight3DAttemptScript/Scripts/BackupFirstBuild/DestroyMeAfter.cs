using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMeAfter : MonoBehaviour {

	public float lifeTime=2f;
	void Start () {
		Destroy(gameObject,lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
