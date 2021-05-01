using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControlAlter : MonoBehaviour 
	{

	public Transform[] walkPoints;
	private int walk_Index=0;
	private Transform playerTarget;
	private Animator anim;
	private NavMeshAgent navAgent;
	private float follow_Distance=15f;
	private float walk_Distance=8f;
	private float attack_Distance=2f;
	private float currentAttackTime;
	private float waitAttackTime=1f;
	private Vector3 nextDestination;
	PlayerHealthScript playerHealth;
    EnemyHealth enemyHealth;
	Vector3 intialPosition_p;
	bool imAlive=true,DeathAnimationComplete=false;
	bool goingBack=false;
	public float angleShift=-30;
	void Awake () 
		{
    	  intialPosition_p=transform.position;
		  playerTarget=GameObject.FindGameObjectWithTag("Player").transform;
		  anim=GetComponent<Animator>();
		  navAgent=GetComponent<NavMeshAgent>();
		  playerHealth=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthScript>();	
		  enemyHealth=GetComponent<EnemyHealth>();
		}
	
		void Update () 
			{
			  if(!imAlive && DeathAnimationComplete) return;
			  if(enemyHealth.health<=0)
			  	{
			  		imAlive=false;
			  	}
			  if(!imAlive)
			  	{
			  		anim.SetBool("Death",true);
					// charController.enabled=false;
					navAgent.enabled=false;
					if( !anim.IsInTransition(0)   
						 && anim.GetCurrentAnimatorStateInfo(0).IsName("Death")
						  && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >=0.95f //animation finishes at 1f
					  )
						{
							Destroy(gameObject,2f);
							DeathAnimationComplete=true;
						}
			  		return ;
			  	}

			  if(!playerHealth.isAlive) return;
			  float distance=Vector3.Distance(transform.position,playerTarget.position);
			  if(Vector3.Distance(transform.position,intialPosition_p)>follow_Distance || goingBack)
			  	{           goingBack=true;
			  				// navAgent.isStopped=false;
			  				anim.SetBool("Walk",false);
			  				anim.SetBool("Run",true);
			  				anim.SetInteger("Atk",0);			  				
			  				navAgent.SetDestination(intialPosition_p);

			  				//newcode
			  				if(navAgent.remainingDistance <= 0.5f)
			  				 goingBack=false;

			  	}

			  else if(distance > walk_Distance)
			  	{
			  		if(navAgent.remainingDistance <= 0.5f)
			  			{
			  				navAgent.isStopped= false;
			  				anim.SetBool("Walk",true);
			  				anim.SetBool("Run",false);
			  				anim.SetInteger("Atk",0);

			  				nextDestination=walkPoints[walk_Index].position;
			  				navAgent.SetDestination(nextDestination);
			  				if(walk_Index==walkPoints.Length-1)
			  					{
			  						walk_Index=0;
			  					}
			  				else 
			  					{
			  						walk_Index++;
			  					}
			  			}
			  	}
			  else
			  	{
			  		if(distance > attack_Distance)
			  			{
			  				navAgent.isStopped=false;
			  				anim.SetBool("Walk",false);
			  				anim.SetBool("Run",true);
			  				anim.SetInteger("Atk",0);			  				
			  				navAgent.SetDestination(playerTarget.position);
			  			}
			  		else
			  			{
			  				navAgent.isStopped=true;
			  				anim.SetBool("Run",false);
			  				Vector3 targetPosition= new Vector3(playerTarget.position.x,transform.position.y,playerTarget.position.z);
			  				transform.rotation=Quaternion.Slerp(transform.rotation,
			  					                Quaternion.Euler(0,-angleShift,0)* Quaternion.LookRotation(targetPosition-transform.position),
			  					                                5f * Time.deltaTime
			  					                                );
			  				if(currentAttackTime>=waitAttackTime)
			  					{
			  						int atkRange=Random.Range(1,3);
			  						anim.SetInteger("Atk",atkRange);
			  						currentAttackTime=0f;
			  					}
			  				else
			  					{
			  						anim.SetInteger("Atk",0);
			  						currentAttackTime+=Time.deltaTime;
			  					}
			  			}
			  	}
			}
	}
