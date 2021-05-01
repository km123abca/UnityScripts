using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MannyAnimationControl : MonoBehaviour 
	{
	Animator anim;
	void Start () 
		{
			anim=GetComponent<Animator>();
		}
	
	
	void Update () 
		{
		
		}
	public void Move(float vel)
		{
			if(Mathf.Abs(vel) > 0)
				{
					anim.SetBool("isRunning",true);
					
				}
			else
				{
					anim.SetBool("isRunning",false);
				}
		}
	public void AnimateJump()
		{
			anim.SetTrigger("jumped");
		}

	// void OnBecameInvisible()
	// 	{
	// 		transform.parent.GetComponent<MannyScript>().DoStuffOnOutOfBounds();
	// 	}


	}
