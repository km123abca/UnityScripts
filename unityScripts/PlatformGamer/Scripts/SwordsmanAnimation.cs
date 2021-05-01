using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanAnimation : MonoBehaviour 
	{
	Animator anim;
	bool attackedAlready;	
	public float maxAttackGap=2f;
	float attackTime=0f;
	void Start () 
		{
		 attackTime=maxAttackGap;
		 anim=GetComponent<Animator>();	
		}
	
	
	void Update () 
		{
		  if(!anim.IsInTransition(0) && 
			   anim.GetCurrentAnimatorStateInfo(0).IsName("sman_attack") &&
			    anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.5f &&
			   !attackedAlready
		     )
			{
				transform.parent.GetComponent<Swordsman>().InflictDamage();
				attackedAlready=true;
			}
		else if(
				 !(anim.GetCurrentAnimatorStateInfo(0).IsName("sman_attack"))
			   )
			{
				attackedAlready=false;
				attackTime=maxAttackGap;				
			}

		}
	public void Move(float vel)
		{
			if(vel > 0)
				anim.SetBool("isWalking",true);
			else
				anim.SetBool("isWalking",false);
		}

	public void StartAttack()
		{
			if(attackTime >= maxAttackGap)
				{
				anim.SetTrigger("attack");
				attackTime=0f;
				}
			else
				{
					attackTime+=Time.deltaTime;
				}
		} 
	}
