using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

	
	public float health=100f;
	float maxHealth;
	public Image health_Img;
	/*
	void Awake()
		{
			maxHealth=health;
			if(tag=="Boss")
				{
					health_Img=GameObject.Find("Health Foreground Boss").GetComponent<Image>();
				}
			else
				{
					health_Img=GameObject.Find("Health FG").GetComponent<Image>();
				}
		}
	*/
	public void TakeDamage(float amount)
		{
			 health-=amount;
			 Debug.Log("health:"+health);
			 health_Img.fillAmount=health/maxHealth;
			 // Debug.Log(health);
			 if(health <= 0)
			 	{
			 		Debug.Log("Enemy Died");
			 	}
		}
}
