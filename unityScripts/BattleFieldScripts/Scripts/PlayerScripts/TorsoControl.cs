using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoControl : MonoBehaviour 
	{

	Animator anim;
	public WrapperControl parentScript;
	bool attacked;
	SpriteRenderer sr;
	void Start () 
		{
		  anim=GetComponent<Animator>();
		  sr=GetComponent<SpriteRenderer>();
		}
	public void TurnOffRendering()
		{
			sr.enabled=false;
		}
	public void TurnOnRendering()
		{
			sr.enabled=true;
		}
	
	void Update () 
		{
				if(!anim.IsInTransition(0) && 
					anim.GetCurrentAnimatorStateInfo(0).IsName("attack_knife") 			    	
			      )
					{
						if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.9f)
							{
								parentScript.ComeOutOfAttack();
								anim.SetBool("attack",false);
							}
						else if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.6f)
							{
								if(!attacked && parentScript.CheckForHitVictims())
									{
										attacked=true;
										// Debug.Log("hit detected");									
									}
							}
					}
				else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("attack_knife") )
					{
						attacked=false;
					}
		}

	public void Attack()
		{
			anim.SetBool("attack",true);
		}
	public void MoveOrStop(float speed)
		{
			if(Mathf.Abs(speed)>0)
				{
					anim.SetBool("isMoving",true);
				}
			else 
					anim.SetBool("isMoving",false);
		}




	}
