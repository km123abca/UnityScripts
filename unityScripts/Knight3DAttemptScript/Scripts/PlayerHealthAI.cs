using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthAI : MonoBehaviour 
	{

	Image healthImage;
	public float maxHealth=100f;
	float health;
	public GameObject healthBarForeground,healthBarBackground;
	public bool shielded;
	public string debugString;
	public bool alive=true;
	public void TakeDamage(float dmg)
		{
			if(!alive) return;
			if(shielded) 
				{
				Debug.Log(gameObject.tag+" is shielded... attack ineffective");
				return;
				}
			health-=dmg;
			// Debug.Log(gameObject.tag+" says, Received Damage health is now:"+health);
			healthBarForeground.SetActive(true);
			healthBarBackground.SetActive(true);
			StartCoroutine(HideHealthBar());
			health=(health<=0)?0:health;
			if(health >= 0)
				healthBarForeground.GetComponent<Image>().fillAmount=health/maxHealth;
			if(health <= 0)
				{
					alive=false;
				}


		}
	IEnumerator HideHealthBar()
		{
			yield return new WaitForSeconds(1.0f);
			healthBarForeground.SetActive(false);
			healthBarBackground.SetActive(false);
		}
	void Start () 
		{ 
			debugString="Hi i am "+gameObject.tag;
			health=maxHealth;
			// Todo get healthbar image and fill it
			// healthImage=GameObject.Find("HealthIcon").GetComponent<Image>();
			// healthImage.fillAmount=health/maxHealth;
		}
	
	
	void Update () 
		{
		
		}
	public void HealPlayer(float healAmount)
		{
			health+=healAmount;
			health=(health >= maxHealth)?maxHealth:health;
			Debug.Log("Health has been increased to:"+health);
			//fill health bar
			// healthImage.fillAmount=health/maxHealth;
		}

	}
