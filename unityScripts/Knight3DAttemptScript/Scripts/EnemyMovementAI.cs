using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyMovementAI : MonoBehaviour 
	{
	public Transform[] walkPoints;
	int walkIndex;
	Animator anim;
	Transform target;
	NavMeshAgent navAgent;
	Vector3 nextDestination,initialPosition;
	public float pointProximityDistance=0.5f;
	public float attackDistance=1f,runDistance=15f,walkDistance=7f,maxDistanceFromHome=18f;
	bool patrolling=true;
	bool goingBack;
	Vector3 lastPosition;

	float attackTimer;
	public float maxAttackGap=3f;
	bool playerDied=false;

	int dummyCounter=0;
	public float angleShift=60,maxHealth=100f;
	float health;
	public GameObject healthBarForeground,healthBarBackground;
	bool dying;
	PlayerAttack pattack;
	public void TakeDamage(float dmg)
		{

			health-=dmg;
			healthBarForeground.SetActive(true);
			healthBarBackground.SetActive(true);
			StartCoroutine(HideHealthBar());
			if(health >= 0)
				healthBarForeground.GetComponent<Image>().fillAmount=health/maxHealth;
			if(health < 0)
				{
					GoIntoDeathAnimations();
				}
		}
	IEnumerator HideHealthBar()
		{
			yield return new WaitForSeconds(1.0f);
			healthBarForeground.SetActive(false);
			healthBarBackground.SetActive(false);
		}
	void GoIntoDeathAnimations()
		{
			if(!dying || !anim.GetBool("Death"))
			{
			  dying=true;
			  anim.SetBool("Death",true);
			}
			Destroy(gameObject,2.05f);
		}
	
	void Start () 
		{
		  attackTimer=maxAttackGap;
		  initialPosition=transform.position;
		  target= GameObject.FindGameObjectWithTag("Player").transform;
		  pattack=target.GetComponent<PlayerAttack>();
		  anim= GetComponent<Animator>();
		  navAgent=GetComponent<NavMeshAgent>();
		  navAgent.SetDestination(initialPosition);
		  health=maxHealth;
		  // healthBarForeground.SetActive(false);
		  // healthBarBackground.SetActive(false);

		}
	
	
	void Update () 
		{
			if(dying) return;	
			if(goingBack || playerDied)				
				Patrol();
			else
				ChasePlayer();

			if(pattack.IsAttacking_info)
				{
					Debug.Log("Attack coming in");
				}
		}

	void Patrol()
		{
			if(goingBack)
			 ReFocusOnCloseByEnemy();

			if(navAgent.remainingDistance <= pointProximityDistance)
				{
					
					 if(goingBack) goingBack=false;
					 navAgent.isStopped=false;
					 anim.SetBool("Walk",true);
					 anim.SetBool("Run",false);
					 anim.SetInteger("Atk",0);

					 nextDestination = walkPoints[walkIndex].position;
					 navAgent.SetDestination(nextDestination);
					 walkIndex= walkIndex+1>=walkPoints.Length?0:walkIndex+1;
				}
		}
	void SetUpReturnToInitPos()
		{
			navAgent.SetDestination(initialPosition);
		}
	void ChasePlayer()
		{
			if(playerDied)
				return;
			if(!target)
				{
				  playerDied=true;
				  SetUpReturnToInitPos();
				  return;				
				}
			Vector3 enemyPos      =new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z);
			if(CameTooFar())
				{
					navAgent.isStopped=false;
				    anim.SetBool("Walk",true);
					anim.SetBool("Run",false);
					anim.SetInteger("Atk",0);
					return;
				}
			float distanceToTarget=Vector3.Distance(transform.position,enemyPos);
			if(distanceToTarget < attackDistance)
				Attack();
			else if(distanceToTarget < runDistance)
				{
				attackTimer=maxAttackGap;
				RunTowardsPlayer();
				}
			else if(distanceToTarget < walkDistance)
				{
				attackTimer=maxAttackGap;
				WalkTowardsPlayer();
				}
			else
				{
				attackTimer=maxAttackGap;
			 	Patrol();
				}
		}
	bool CameTooFar()
		{
			float distanceFromHome=Vector3.Distance(transform.position,initialPosition);
			if(distanceFromHome > maxDistanceFromHome)
				{
					SetUpReturnToInitPos();
					goingBack=true;
					lastPosition=transform.position;
					return true;
				}
			return false;
		}

	void ReFocusOnCloseByEnemy()
		{
			Vector3 enemyPos =new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z);
			float distanceToLastPoint=Vector3.Distance(transform.position,lastPosition);
			float distanceToEnemy= Vector3.Distance(transform.position,enemyPos);
			if(distanceToEnemy < distanceToLastPoint)
				{
					goingBack=false;
				}
		}

	void RunTowardsPlayer()
		{
			attackTimer=maxAttackGap;
			navAgent.isStopped=false;
			anim.SetBool("Walk",false);
			anim.SetBool("Run",true);
			anim.SetInteger("Atk",0);			  				
			navAgent.SetDestination(target.position);
		}
	void WalkTowardsPlayer()
		{
			navAgent.isStopped=false;
			anim.SetBool("Walk",true);
			anim.SetBool("Run",false);
			anim.SetInteger("Atk",0);			  				
			navAgent.SetDestination(target.position);
		}

	void FaceTarget()
		{
			if(!target)
				return;
			Vector3 targetPosition= new Vector3(target.position.x,transform.position.y,target.position.z);
			transform.rotation=Quaternion.Slerp(transform.rotation,
												Quaternion.Euler(0,-angleShift,0)* 
				                                Quaternion.LookRotation(targetPosition-transform.position),
				                                5f * Time.deltaTime
				                                );
		}
	void Attack()
		{
		 navAgent.isStopped=true;
		 FaceTarget();
		 anim.SetBool("Walk",false);
		 anim.SetBool("Run",false);		 
		 attackTimer+=Time.deltaTime;
		 if(attackTimer > maxAttackGap)
		 	{
		 		
		 		int atkRange=Random.Range(1,3);
		 		attackTimer=0f;
		 		anim.SetInteger("Atk",atkRange);		
		 	}
		 else
		 	{
		 		anim.SetInteger("Atk",0);		
		 	}
		 
		}



	}
