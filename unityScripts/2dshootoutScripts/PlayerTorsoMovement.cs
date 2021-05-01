using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorsoMovement : MonoBehaviour {

	Animator anim;
	int shotsFired=0;
	public int maxShots=9;
	PlayerMovement parentScript;
	EnemyMovement parentScript_alter;
	// EnemyMovement parentScript1;
	void Start () {
		anim=GetComponent<Animator>();		
		parentScript=transform.parent.GetComponent<PlayerMovement>();
		
		parentScript_alter=transform.parent.GetComponent<EnemyMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!anim.IsInTransition(0) && 
			anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack") &&
			    anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.5f &&
			   shotsFired < maxShots
		      )
			{
				shotsFired+=1;
				if(shotsFired %5==0)
					{
					if(parentScript)
						parentScript.SpawnBullet();
					else
						parentScript_alter.SpawnBullet();
					}
			}
		else if(shotsFired!=0)
			{
				shotsFired=0;
			}
			
	}
	public void moveOrStop(float speed)
		{
			if(Mathf.Abs(speed)>0)
				{
					anim.SetBool("moving_t",true);
				}
			else 
					anim.SetBool("moving_t",false);
		}
	public void Attack()
		{
			if(shotsFired < maxShots)
			anim.SetTrigger("startAttack");
		}
}
