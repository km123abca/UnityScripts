using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour 
	{

	Image healthImage;
	public float maxHealth=100f;
	float health;
	public void TakeDamage(float dmg)
		{

			health-=dmg;
			Debug.Log(gameObject.tag+" took damage health:"+health);
			health=(health<=0)?0:health;
			healthImage.fillAmount=health/maxHealth;

		}
	void Start () 
		{
			health=maxHealth;

			//have to enable this to take care of 
			
			healthImage=GameObject.Find("HealthIcon").GetComponent<Image>();
			healthImage.fillAmount=health/maxHealth;
			
		}
	
	
	void Update () 
		{
		
		}
	public void HealPlayer(float healAmount)
		{
			health+=healAmount;
			health=(health >= maxHealth)?maxHealth:health;
			Debug.Log("Health has been increased to:"+health);
			healthImage.fillAmount=health/maxHealth;
		}

	}
