using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
	{
	public static GameManager instance;
	public GameObject overText;
	public bool gameOver;
	void OnEnable()
		{
			SceneManager.sceneLoaded += LevelFinishedLoading;//suscribing to an event
		}
	void OnDisable()
		{

			SceneManager.sceneLoaded -= LevelFinishedLoading;
		}
	void LevelFinishedLoading(Scene scene,LoadSceneMode mode)
		{
			Debug.Log("I was called");
			overText.GetComponent<Text>().text="";
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
	void Awake()
		{
			MakeSingleton();
		}
	void Start () 
		{
		GameObject.Find("FinalText").GetComponent<Text>().text="";
		
		}	
	void Update () 
		{
			if(gameOver)
				{
					GameObject.Find("FinalText").GetComponent<Text>().text="YOU DIED, Press Space";
					// gameOver=false;
				}
			if(gameOver && Input.GetButtonDown("Jump"))
				{
					gameOver=false;
					GameObject.Find("FinalText").GetComponent<Text>().text="";
					SceneManager.LoadScene("MainScene");
				}

		}
	public void RestartGame()
		{
			SceneManager.LoadScene("MainScene");
		}
	public bool GameOver	
		{
			get {return gameOver;}
			set { gameOver=value;}
		}

	}
