using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour 
	{
	Animator anim;
	bool canAttack;
	void Start () 
		{
		  anim=GetComponent<Animator>();
		}
	
	
	void Update () 
		{
		if(!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Stand") )
			{
			canAttack=true;
			}
		else
			canAttack=false;
		}
		
	}
