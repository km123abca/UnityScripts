using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour {

	public GameObject gameStartedPosition;
	public GameObject characterSelectPosition;
	private bool reached_GameStartedPosition;
	bool reached_CharacterSelectPosition=true;
	private bool canClick;
	private bool backToMainMenu;
	private List<GameObject> positions= new List<GameObject>();

	void Awake()
		{
			positions.Add(gameStartedPosition);
		}
	void MoveToPosition()
		{
			if(positions.Count > 0)
				{
					transform.position=Vector3.Lerp(
													transform.position,
													positions[0].transform.position,
													1f*Time.deltaTime
												   );
					transform.rotation=Quaternion.Lerp(
														transform.rotation,
														positions[0].transform.rotation,
														1f * Time.deltaTime
													  );
				}
		}
	public void ChangePosition(int index)
		{
			positions.RemoveAt(0);
			if(index==0)
				{
					positions.Add(gameStartedPosition);
				}
			else
				{
					positions.Add(characterSelectPosition);
				}
		}
	
	void Update () {
		/*
		MoveToGameStartedPosition();
		MoveToCharacterSelectMenu();
		MoveBackToMainMenu();
		*/
		MoveToPosition();
	}

	void MoveBackToMainMenu()
		{
			if(backToMainMenu)
				{
					if(Vector3.Distance(transform.position,gameStartedPosition.transform.position)<0.2f)
						{
							backToMainMenu=false;
							canClick=true;
						}
					if(backToMainMenu)
						{
							transform.position=Vector3.Lerp(transform.position,
															gameStartedPosition.transform.position,
															Time.deltaTime
														    );
							transform.rotation=Quaternion.Lerp(transform.rotation,
															   gameStartedPosition.transform.rotation,
															   Time.deltaTime
															   );
						}
				}
		}
	public bool ReachedCharacterSelectPosition
		{
			get 
				{
					return reached_CharacterSelectPosition;
				}
			set 
				{
					reached_CharacterSelectPosition=value;
				}
		}
	public bool CanClick 
		{
			get
				{
					return canClick;
				}
			set
				{
					canClick=value;
				}
		}
	public bool BackToMainMenu
		{
			get { return backToMainMenu;}
			set
				{
					backToMainMenu=value;
				}
		}
	void MoveToGameStartedPosition()
		{
			if(!reached_GameStartedPosition)
				{
					if(Vector3.Distance(transform.position,gameStartedPosition.transform.position) < 0.2f)
						{
							reached_GameStartedPosition=true;
							canClick=true;
						}
					if(!reached_GameStartedPosition)
						{
							transform.position=Vector3.Lerp(transform.position,
															gameStartedPosition.transform.position,
															Time.deltaTime
														    );
							transform.rotation=Quaternion.Lerp(transform.rotation,
															   gameStartedPosition.transform.rotation,
															   Time.deltaTime
															   );
						}
				}
		}
	void MoveToCharacterSelectMenu()
		{
			if(!reached_CharacterSelectPosition)
				{
					transform.position=Vector3.Lerp(transform.position,
													characterSelectPosition.transform.position,
													Time.deltaTime
												   );
					transform.rotation = Quaternion.Lerp(transform.rotation,
														 characterSelectPosition.transform.rotation,
														 Time.deltaTime
														);
					if(Vector3.Distance(transform.position,characterSelectPosition.transform.position) < 0.2f)
						{
							reached_CharacterSelectPosition=true;
							canClick=true; 
						}
				}
		}
}
