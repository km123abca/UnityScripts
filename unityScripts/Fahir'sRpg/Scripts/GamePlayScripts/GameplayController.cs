using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public void LoadOtherWorld()
		{
			string name=UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
			SceneLoadeer.instance.LoadLevel(name);
		}
}
