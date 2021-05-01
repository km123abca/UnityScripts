using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetControl : MonoBehaviour 
	{

	Animator anim;
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
		
		}
	public void MoveOrStop(float speed)
		{
			if(Mathf.Abs(speed)>0)
				{
					anim.SetBool("torsoMoving",true);
				}
			else 
					anim.SetBool("torsoMoving",false);
		}
	}
