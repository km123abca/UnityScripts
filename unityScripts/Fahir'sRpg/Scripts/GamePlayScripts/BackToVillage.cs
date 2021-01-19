using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToVillage : MonoBehaviour {

	void OnTriggerEnter(Collider target)
		{
			if(target.tag=="Player")
				{
					SceneLoadeer.instance.LoadLevel("Village"); 
				}
		}
}
