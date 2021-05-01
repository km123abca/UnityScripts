using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimations : MonoBehaviour {

	Animator anim;
	float attackTimer=0f;
	public float maxAttackLimit=3f;
	bool attackedAlready;
	void Start () {
		attackTimer=maxAttackLimit;
		anim=GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!anim.IsInTransition(0) && 
			anim.GetCurrentAnimatorStateInfo(0).IsName("Zombie_Attack") &&
			    anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.5f &&
			   !attackedAlready
		     )
			{
				transform.parent.GetComponent<ZombieMovement>().InflictDamage();
				attackedAlready=true;
			}
		else if((anim.GetCurrentAnimatorStateInfo(0).IsName("Zombie_Idle") ||
			    anim.GetCurrentAnimatorStateInfo(0).IsName("Zombie_Move"))
					&& (attackedAlready || attackTimer!=maxAttackLimit)
			    )
			{
				attackedAlready=false;
				attackTimer=maxAttackLimit;
			}

	}
	public void Move(Vector2 magn)
		{
			if(magn.magnitude > 0)
				{
					anim.SetBool("zombieIsMoving",true);
					anim.speed=magn.magnitude/2.5f;
				}
			else
				{
					anim.SetBool("zombieIsMoving",false);
				}
		}
	bool CanAttack()
		{
			if(attackTimer >= maxAttackLimit) 
				{
				attackTimer=0f;
				return true;
				}
			attackTimer+=Time.deltaTime;
			return false;
		}
	public void Attack()
		{
			if(CanAttack())
			anim.SetTrigger("zombieAttack");
		}
}
