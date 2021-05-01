using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour {

	Rigidbody2D rb;
	public float rotationSpeed,playerSpeed;
	public GameObject p_feet,p_torso,bulletObj,bulletObjPos;
	public float maxHealth=100f;
	
	float health=0f;

	public void IncreaseHealth(float x)
		{
			if(health < maxHealth)
			health+=x;
			GameObject.Find("greenbar_image").GetComponent<Image>().fillAmount=health/maxHealth;
		}
	public void ReplenishHealth()
		{
			health=maxHealth;
		}
	public void TakeDamage(float dmg)
		{
			// return;
			health-=dmg;
			if(health >= 0)
				GameObject.Find("greenbar_image").GetComponent<Image>().fillAmount=health/maxHealth;
			if(health <= 0)
				{
					GameObject.Find("PlayerDeathText").GetComponent<Text>().text="YOU DIED, PRESS SPACE";
					GameObject.Find("GameManager").GetComponent<GameManager>().gameEnded=true;
					Destroy(gameObject);
				}
		}

	void Start () {		
		health=maxHealth;
		if(SceneManager.GetActiveScene().name=="BossFight")
				ReplenishHealth();
		GameObject.Find("greenbar_image").GetComponent<Image>().fillAmount=health/maxHealth;
		
		rb=GetComponent<Rigidbody2D>();
		// Debug.Log(rb.velocity);
		
	}
	
	
	void Update () {
		float horizInput=-1*Input.GetAxis("Horizontal");
		float vertInput = Input.GetAxis("Vertical");		
		transform.rotation=Quaternion.Euler(0,0,horizInput * rotationSpeed) * transform.rotation;
		
		
			
		
		
		rb.velocity=transform.rotation * Vector2.right * playerSpeed * vertInput;

		//animate torso and feet according to vertical input 
		p_feet.GetComponent<PlayerFeetMovement>().moveOrStop(vertInput);
		p_torso.GetComponent<PlayerTorsoMovement>().moveOrStop(vertInput);

		if(Input.GetButtonDown("Jump"))
			Attack();
	}

	void Attack()
		{
			p_torso.GetComponent<PlayerTorsoMovement>().Attack();	
		}
	public void SpawnBullet()
		{
			GameObject g=Instantiate(bulletObj,bulletObjPos.transform.position,transform.rotation);
			g.SendMessage("GetParent",gameObject);
			// g.SendMessage("getRotation",transform.rotation);
		}
	
}
