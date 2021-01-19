using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadeer : MonoBehaviour {

	public static SceneLoadeer instance;
	[SerializeField]
	GameObject loadingScreen;
	string levelName;

	void MakeSingleton()
		{
			if(instance!=null)
				{
					Destroy(gameObject);
				}
			else
				{
					instance=this;
					DontDestroyOnLoad(gameObject);
				}
		}

	void Awake () {
		MakeSingleton();
	}
	
	public void LoadScene(string sceneName)
		{
			SceneManager.LoadScene(sceneName);	
		}


	/*
	public void LoadScene(string sceneName)
		{
			loadingScreen.SetActive(true);
			SceneManager.LoadScene(sceneName);
		}
	*/

	public void LoadLevel(string name)
		{
			levelName=name;
			StartCoroutine(LoadLevelWithName());
		}
	IEnumerator LoadLevelWithName()
		{
			loadingScreen.SetActive(true);
			SceneManager.LoadScene(levelName);
			yield return new WaitForSeconds(2.5f);
			loadingScreen.SetActive(false);
		}
}
