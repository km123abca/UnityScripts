using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour {

	public float health;
	public float max_health=100f;
	public bool isShielded;
	public string welcomeString="hello";
	Animator anim;
	public bool isAlive=true;
	public bool Shielded
		{
			get {return isShielded;}
			set {isShielded=value;}
		}
	private Image health_Img;
	public void TakeDamage(float amount)
		{
			if(!isShielded)
				{
				health-=amount;
				health_Img.fillAmount=health/max_health;
				
				if(health <= 0f)
					{
						anim.SetBool("Death",true);
						isAlive=false;
						if(!anim.IsInTransition(0) && 
							anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && 
							anim.GetCurrentAnimatorStateInfo(0).normalizedTime >=0.95f
					      )
							{
								//do something after destruction
							}
				
							
				   }
				  }
			
		}

	void Awake () 
		{
			anim=GetComponent<Animator>();
			health_Img=GameObject.Find("Health Icon").GetComponent<Image>();
			health=max_health;
		}
	
	
	void Update () 
		{
		 
		}
	public void HealPlayer(float healAmount)
		{
			health+=healAmount;
			health=health>100f?100f:health;
			health_Img.fillAmount=health/max_health;
		}
}
