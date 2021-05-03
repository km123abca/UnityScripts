using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAttackAI : MonoBehaviour 
	{	
		float[] attackTimers=new float[]{0f,0f,0f,0f,0f,0f};
		public float[] attackTimersMax=new float[]{0f,0f,0f,0f,0f,0f};
		Animator anim;
		bool canAttack=true;
		public bool isAttacking;
		CharacterMovement charMoveScript;


		void RunAttackTimers()
			{
				for(int i=0;i < attackTimers.Length;i++)
					{
						if(attackTimers[i]==0f) continue;
						attackTimers[i]+=Time.deltaTime;
						if(attackTimers[i] > attackTimersMax[i])
							{
								attackTimers[i]=0f;
							}
					}
			}

		void ResetToStandIfNotAttacking()
			{
				if(isAttacking && true) 
				anim.SetInteger("Atk",0);
			}
		void Awake () 
			{

			  anim=GetComponent<Animator>();
			  charMoveScript=GetComponent<CharacterMovement>();
			}
	
	

		void Update () 
			{
				RunAttackTimers();
				

				if(!(!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Stand") ))
					isAttacking=true;
				else
					isAttacking=false;
				ResetToStandIfNotAttacking();
				if(!anim.IsInTransition(0))
					canAttack=true;
				else
					canAttack=false;			
				
			}

		public void PlayDeathAnimation()
			{
				anim.SetBool("Death",true);
			}

		public bool IsAttacking_info
			{
				get {return isAttacking;}
			}
		void Attack(int a) //a can range from 1 to 6
			{				
				if(attackTimers[a-1]==0f)
					{
					anim.SetInteger("Atk",a);
					attackTimers[a-1]+=Time.deltaTime;
					}
				 

			}
		bool CanIAttack(int n)
			{
				if(n <1 || n >attackTimers.Length)
					return false;
				return attackTimers[n-1]==0f;

			}
		public bool CanShieldUp()
			{
				if(CanIAttack(4))
					{
					Attack(4);
					return true;
					}
				return false;
			}


	    //1 -- groundimpact
		//2 -- Kick
		//3 -- Fire Tornado
		//4 -- FireShield
		//5 -- Heal
		//6 -- Thunder Attack
		public void LaunchRandomMinorAttack()
			{
				int r_x= Random.Range(1,3);		

				for(int i=0;i < 2;i++)
					{

						if( CanIAttack(r_x))
							{
								Attack(r_x);
								return;
							}
						r_x=(r_x+1 > 2)?1:r_x+1;

					}

			}
		public void LaunchRandomMajorAttack()
			{
				int r_x= Random.Range(1,3);		

				for(int i=0;i < 2;i++)
					{

						if(CanIAttack(r_x*3))
							{
								Attack(r_x*3);
								return;
							}
						r_x=(r_x+1 > 2)?1:r_x+1;

					}

			}
		

	}

