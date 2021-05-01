using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingDamage : MonoBehaviour {

	public float destroyAfterTime=4f;
	
	void Start () {

		
		Destroy(gameObject,destroyAfterTime);
	}
	void Update(){
		
	}
	
}
