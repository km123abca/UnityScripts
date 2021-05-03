using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToradoFireMovement : MonoBehaviour 
	{

	public float speed=5f;
	public GameObject explosionAtEnd;

	LayerMask enemyLayer;
	public float radius =0.5f;
	public float damageCount=10f;
	EnemyMovementAI enemyScript;
	bool collided;

	public float maxLife=2f;
	float secondsFromBirth=0f;

	void Start () 
		{
		GameObject player= GameObject.FindGameObjectWithTag("Player");
		transform.rotation=Quaternion.LookRotation(player.transform.forward);
		}
	
	
	void Update () 
		{
		  Move();
		  CheckLife();
		  CheckForDamage();
		}
	void GetEnemyLayer(LayerMask el)
		{
			enemyLayer=el;
		}
	

	void CheckLife()
		{
			secondsFromBirth+=Time.deltaTime;
			if(secondsFromBirth > maxLife)
				{
					Vector3 tempPosition=transform.position;
					tempPosition.y+=2f;
					Instantiate(explosionAtEnd,tempPosition,Quaternion.identity);
					Destroy(gameObject);
				}
		}

	void Move()
		{
			transform.Translate(Vector3.forward* speed * Time.deltaTime);
		}
	void CheckForDamage()
		{
			Collider[] hits=Physics.OverlapSphere(transform.position,radius,enemyLayer);
			foreach(Collider c  in hits)
				{
				if(c.isTrigger)	continue;
				enemyScript=c.gameObject.GetComponent<EnemyMovementAI>();
				collided=true;
				}
			if(collided)
				{
					enemyScript.TakeDamage(damageCount);
					Vector3 tempPosition=transform.position;
					tempPosition.y+=2f;
					Instantiate(explosionAtEnd,tempPosition,Quaternion.identity);
					Destroy(gameObject);
				}
		}
	}
