using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {

	private Animator anim;
	void Awake () {
		anim=GetComponent<Animator>();
	}
	
	
	void Update () {
		if(Input.GetKey(KeyCode.W))
			{
				anim.SetBool("Walk",true);
				anim.SetFloat("Blend",1);
			}
		else
			{
			    anim.SetBool("Walk",false);	
			    anim.SetFloat("Blend",0);
			}
	}
}
