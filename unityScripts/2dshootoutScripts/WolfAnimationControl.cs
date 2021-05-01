using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimationControl : MonoBehaviour 
	{
	Animator anim;
	public float maxAttackLimit=3f;
	float attackTimer;
	bool attackedAlready,teleported;
	public GameObject rc;
	void Start ()
	 	{
	 	  attackTimer=maxAttackLimit;
		  anim=GetComponent<Animator>();

		}
	
	
	void Update () 	
		{
			//****************** Checking for Attack **********************
			if(!anim.IsInTransition(0) &&
			    anim.GetCurrentAnimatorStateInfo(0).IsName("wolfAttack") &&
			    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f &&
			    !attackedAlready
			  )
				{
					//attack here
					transform.parent.GetComponent<WolfLordMovement>().InflictDamage();
					attackedAlready=true;
				}
			else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("wolfAttack") && attackedAlready)
				attackedAlready=false;
			//*****************************************************************

			//**************** Checking for Teleport **************************
			if(!anim.IsInTransition(0) &&
			    anim.GetCurrentAnimatorStateInfo(0).IsName("wolfDying") &&
			    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f 
			  	&& !teleported
			  )
				{
				transform.parent.GetComponent<WolfLordMovement>().TeleportNearTarget();
				teleported=true;
				}
			else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("wolfDying") && teleported)
				teleported=false;
			//******************************************************************
		}
	public void Move(Vector2 magn)
		{
			if(magn.magnitude > 0)
				{
					anim.SetBool("wolfIsMoving",true);
					anim.speed=magn.magnitude/2f;
				}
			else
				{
					anim.SetBool("wolfIsMoving",false);
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
				{
				anim.SetTrigger("wolfAttack");
				Instantiate(rc,transform.position,Quaternion.identity);
				}
		}
	public void DisappearNTeleport()
		{
			anim.SetTrigger("wolfDeath");
		}
	}
