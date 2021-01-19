using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : MonoBehaviour {


	public GameObject[] characters;
	public GameObject charPosition;
	int knight_Warrior_Index = 0;
	int king_Warrior_Index = 1;
	int catGirl_Warrior_Index =2;
	void Start () {
		characters[knight_Warrior_Index].SetActive(true);
		characters[knight_Warrior_Index].transform.position = charPosition.transform.position;
	}
	
	public void SelectCharacter()
		{
			TurnOffCharacters();
			characters[int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name)].SetActive(true);
			characters[int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name)].transform.position=charPosition.transform.position;
			GameManager.instance.selectedCharacterIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name); 
		}
	void TurnOffCharacters()
		{
			for(int i=0;i < characters.Length;i++)
				{
					characters[i].SetActive(false);
				}
		}
}
