using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject secondSetOfGifts;
	public bool gameEnded;
	public bool startGame;
	public static GameManager instance;
	bool phase1Complete;

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
	void Start () {
		GameObject.Find("PlayerDeathText").GetComponent<Text>().text="";
		phase1Complete=false;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(SceneManager.GetActiveScene().name=="MainScene")
			{
			if(NoGiftRemains() ) 
				{
					if(!phase1Complete)
					{
					RemoveRoadBlocks();	
					phase1Complete=true;
					}
					else
					{
						ClearWayToPortal();
					}
				}
					if(Input.GetKeyDown(KeyCode.A) )				
					SceneManager.LoadScene("BossFight");			
			}

		
		if(gameEnded && Input.GetButtonDown("Jump"))
			{
				gameEnded=false;
				// startGame=false;
				
				SceneManager.LoadScene("MainScene");
			}

	}

	public void StartGame()
		{
			startGame=true;
			if(SceneManager.GetActiveScene().name=="MainScene")
			GameObject.Find("StartPanel").SetActive(false);
			
		}
	bool NoGiftRemains()
		{
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("gift"))
				{
					return false;
				}
			
			return true;
		}
	void RemoveRoadBlocks()
		{
			Debug.Log("called");
			if(GameObject.Find("blockingbush1")){
			GameObject.Find("blockingbush1").SetActive(false);
			GameObject.Find("blockingbush2").SetActive(false);
			secondSetOfGifts.SetActive(true);
												}
		}
	void ClearWayToPortal()
		{
			if(GameObject.Find("portalBush1"))
				{
					for(int i=1;i <= 6;i++)
						{
							GameObject.Find("portalBush"+i).SetActive(false);
						}
				}
		}
}
