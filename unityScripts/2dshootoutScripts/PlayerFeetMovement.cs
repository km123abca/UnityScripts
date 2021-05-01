using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetMovement : MonoBehaviour {

	Animator anim;
	void Start () {
		anim=GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void moveOrStop(float speed)
		{
			if(Mathf.Abs(speed)>0)
				{
					anim.SetBool("moving",true);
				}
			else 
					anim.SetBool("moving",false);
		}
}
