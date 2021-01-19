using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

	Animator anim;
	string SLEEP_END_ANIMATION="sleep_end";
	string IDLE_ANIMATION="idle";
	string WALK_PARAMETER="Walk";
	string ATTACK1_PARAMETER="Attack1";
	string ATTACK2_PARAMETER="Attack2";
	string JUMP_PARAMETER="Jump";
	void Awake () 
		{
		anim=GetComponent<Animator> ();
		}
	
	public void PlayerWalks(bool walk)
		{
			anim.SetBool(WALK_PARAMETER,walk);
		}
	void EndSleep () 
		{
			anim.Play(SLEEP_END_ANIMATION);
		}
	void BeginIdle()
		{
			anim.Play(IDLE_ANIMATION);
		}
	public void Attack1()
		{
			anim.SetBool(ATTACK1_PARAMETER,true);
		}
	public void EndAttack1()
		{
			
			anim.SetBool(ATTACK1_PARAMETER,false);	
		}
	public void Attack2()
		{
			anim.SetBool(ATTACK2_PARAMETER,true);
		}
	public void EndAttack2()
		{
			anim.SetBool(ATTACK2_PARAMETER,false);	
		}
	public void Jumped()
		{
			anim.SetBool(JUMP_PARAMETER,true);
		}
	public void EndJump()
		{
			anim.SetBool(JUMP_PARAMETER,false);
		}
}
