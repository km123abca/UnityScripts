using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	[SerializeField]
	GameObject[] characters;

	[HideInInspector]
	public int selectedCharacterIndex=0;


	void Awake () {
		MakeSingleton();
	}
	void OnEnable()
		{
			SceneManager.sceneLoaded += LevelFinishedLoading;//suscribing to an event
		}
	void OnDisable()
		{
			SceneManager.sceneLoaded -= LevelFinishedLoading;
		}
	
	void MakeSingleton()
		{
			 if(instance != null)
			 	{
			 		Destroy(gameObject);
			 	}
			 else
			 	{
			 		instance=this;
			 		DontDestroyOnLoad(gameObject);
			 	}

		}
	void LevelFinishedLoading(Scene scene,LoadSceneMode mode)
		{
			if(scene.name != "MainMenu")
				{

					Vector3 pos=GameObject.FindGameObjectWithTag("SpawnPosition").transform.position;
					Debug.Log(pos.x+","+pos.y+","+pos.y);
					Instantiate(characters[selectedCharacterIndex],pos,Quaternion.identity);
				}
		}
}
