using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugTorso : MonoBehaviour 
	{

	Animator anim;
	
	void Start () 
		{
		  anim=GetComponent<Animator>();
		  
		}		
	

	void Update () 
		{
		
		}
	public void AnimateBug(float magn)
		{
			if(magn > 0)
				anim.SetBool("isRunning",true);
			else
				anim.SetBool("isRunning",false);
		}

	}
