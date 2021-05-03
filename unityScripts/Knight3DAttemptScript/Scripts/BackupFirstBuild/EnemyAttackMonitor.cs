using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackMonitor : MonoBehaviour {

	bool didNotAttackYet=true;
	public GameObject AttackCube;
	public GameObject cubeSpawnPosition;
	Animator anim;
	void Awake () 
		{
			anim=GetComponent<Animator>();
		}
	
	
	void Update () 
		{
			if(!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
						{
							didNotAttackYet=true;
						}
			InflictDamageOnTargets();
		}

	void InflictDamageOnTargets()
		{
			
			if(!anim.IsInTransition(0) && 
				anim.GetCurrentAnimatorStateInfo(0).IsName("Atk1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Atk2")
				)
				if(didNotAttackYet && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >=0.5f)
					{
						didNotAttackYet=false;
						// Debug.Log("Damage");
						Instantiate(AttackCube,cubeSpawnPosition.transform.position,Quaternion.identity);
						
					}
			
		}
}
