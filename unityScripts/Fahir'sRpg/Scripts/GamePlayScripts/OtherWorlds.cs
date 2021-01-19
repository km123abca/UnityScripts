using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherWorlds : MonoBehaviour {

	public GameObject loadBtn;

	void OnTriggerEnter(Collider target)
		{
			if(target.tag == "Player")
				{
					loadBtn.SetActive(true);
				}
		}
	void OnTriggerExit(Collider target)
		{
			if(target.tag == "Player")
				{
					loadBtn.SetActive(false);
				}
		}
}
