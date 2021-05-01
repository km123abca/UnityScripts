using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BugWrapper : MonoBehaviour 
	{

	Rigidbody2D rb;
	public GameObject bulletSpawnPos;
	public GameObject bulletx,fireProjectile;
	public float attackRotationSpeed=5f;
	bool doingBulletShower;
	public float maxBulletShowerTime=5f;
	float bulletShowerTime,angleCounter;
	public float maxHalfAngle=20f;
	public float angleInterval=2f;
	float presentAngleDiff=0f;
	public float quickBulletGap=0.5f;
	float quickBulletTimer=0f;
	bool poofed;
	public GameObject cloudPoof,poofPosition;
	float attackStartTimer;
	public float maxAttactStartTimer=3f;
	GameObject target;
	public string enemyString="Player";
	public float bugSpeed=5f;
	public float minGapWithTarget=3f;
	public BugTorso torsoScript;
	bool plunging;
	bool plungePhase1Completed,plungePhase2Completed,focussedOnPlunge,plungeDirectionDecided;
	public float plungeSpeed=10f;
	float plungeCounter;
	public float maxPlungeTimePhase1=0.5f,maxPlungeTimePhase2=1f,maxPlungeTimePhase3=1.5f;
	public float ssShootTime=3f;
	float attackCounter=0f;
	public float maxAttackCounterTime=0.5f;
	bool stayingAndShooting;

	public bool isAI=true;
	public GameObject redBarAI,greenBarAI;
	public float maxHealth=100f;
	float health;
	public float rayCastDistance=10f;
	public float plungeDamage=30f;
	// public float testoffset=90;
	float attackIdleTimer=0f;
	public float maxTimeElpasedIdle=5f;
	public float turnSpeed=1f,quickFocus=10f;
	float bulletTimerWaitTime=0f;
	public float maxBulletTimerWaitTime=5f;
	public int maxFires=4;
	//variables for proximity and far check and fireshower starts
	public float enemyProxForFireShower=20f,maxWatchOutClockTime=3f;
	float watchOutClock;
	float watchOutClock_far;
	public float maxWatchOutClockTime_far=3f;
	//variables for proximity and far check and fireshower ends

	//laser related vars starts
	public GameObject Laserx;
	bool laserShot;
	bool doingLaserFire;
	//laser related vars ends

	bool alignmentReadyForShower;


	int debugCounter=0;

	bool revealed=true;


	public bool IsConspicous
		{
			get {return revealed;}
		}

	//laser related functions starts
	public void ShootLaser()
		{
			GameObject go=Instantiate(Laserx,bulletSpawnPos.transform.position,Quaternion.identity);
			go.transform.SetParent(transform);
		}
	public void InitiateLaserAttack()
		{
			doingLaserFire=true;
		}
//l
	void DoLaserFire()
		{

			if(!poofed)
				{
					Instantiate(cloudPoof,poofPosition.transform.position,Quaternion.identity);				
					poofed=true;
				}

			attackStartTimer+=Time.deltaTime;
			if(attackStartTimer < maxAttactStartTimer)
				{
					return;
				}
			FocusOnTarget();
			if(!alignmentReadyForShower && IsFacingTarget())
				{
					alignmentReadyForShower=true;
				}
			if(!alignmentReadyForShower)
				{
				bulletTimerWaitTime+=Time.deltaTime;
				if(bulletTimerWaitTime > maxBulletTimerWaitTime)
					{
						bulletTimerWaitTime=0f;
						alignmentReadyForShower=true;
					}
				else					
					return;
				}


			bulletShowerTime+=Time.deltaTime;
			if(bulletShowerTime > maxBulletShowerTime)
				{
					bulletShowerTime=0f;
					//laser specific
					doingLaserFire=false;
					laserShot=false;
					//laser specific
					presentAngleDiff=0f;
					poofed=false;
					attackStartTimer=0f;
					alignmentReadyForShower=false;

					return;
				}
			rb.velocity=Vector2.zero;
			if(!laserShot)
				{
				ShootLaser();
				laserShot=true;
				}
			transform.rotation= Quaternion.Euler(0,0,presentAngleDiff)*transform.rotation;
			if ((angleInterval > 0 && presentAngleDiff > maxHalfAngle) 
				||
				(angleInterval < 0 && presentAngleDiff < -maxHalfAngle)
			   )
				{				
					angleInterval*=-1;
				}
			presentAngleDiff+=angleInterval;

			/*
			quickBulletTimer+=Time.deltaTime;
			if(quickBulletTimer > quickBulletGap)
				{
					quickBulletTimer=0f;
					Shoot();
				}
			*/

		}
//l
	//laser related functions ends

	public bool CheckHitTargetsWithName(string nm)
		{
			RaycastHit2D[] hits=Physics2D.RaycastAll(transform.position,
									Quaternion.Euler(0,0,90)*transform.rotation* Vector2.right,
													 rayCastDistance);
			Debug.DrawRay(transform.position,
				         Quaternion.Euler(0,0,90)*transform.rotation* Vector2.right*rayCastDistance,
				          Color.red);
			foreach(RaycastHit2D hitx in hits)
                {
                	if(hitx.collider.gameObject.name==nm)
                		{   
                		Debug.Log("Hit:"+nm);
                		return true;    
                		}           	
                } 
            return false;
		}
	public bool IsAI
		{
			get {return isAI;}
		}
	public void TakeDamageAI(float dmg,bool heavy=true)
		{
			// Debug.Log("damage received, health:"+health);
			health-=dmg;
			redBarAI.gameObject.SetActive(true);
			greenBarAI.gameObject.SetActive(true);			
			if(health>=0)
				{
				// Debug.Log("Filling Done:"+(health/maxHealth*100));
				greenBarAI.GetComponent<Image>().fillAmount=health/maxHealth;
				}
			StartCoroutine(hideHealthBar());
			//insert stagger script here
			if(health < 0)
				{
					Debug.Log("Bug health has gone below zero");
					Instantiate(cloudPoof,transform.position,Quaternion.identity);
					Destroy(gameObject);
				}
		}
	IEnumerator hideHealthBar()
		{
			yield return new WaitForSeconds(1.0f);
			greenBarAI.gameObject.SetActive(false);
			redBarAI.gameObject.SetActive(false);
		}

	void Start () 
		{
			health=maxHealth;
			attackCounter=maxAttackCounterTime;
			rb=GetComponent<Rigidbody2D>();				
			ScanArea();		
			// FireShower();

			InitiateLaserAttack();
			
		}
	
	
	void Update () 
		{
			// Debug.Log(Random.Range(0,2));
			if(doingBulletShower)
				{
					DoBulletShower();
				}
			else if(plunging)
				{
					DoPlunge(); 
				}
			else if(stayingAndShooting)
				{
					StayAndShoot();
				}
			else if(doingLaserFire)
				{
					DoLaserFire();
				}
			else
				{
					WatchOutForCloseByEnemy();
					WatchOutForFarByEnemy();
					FocusOnTarget();
					MoveTowardsTarget();
				}
			torsoScript.AnimateBug(rb.velocity.magnitude);

		}


	void InitiateStayAndShoot()
		{
			if(!target) return;
			stayingAndShooting=true;
		}
    void InitiatePlunge()
    	{
    		if(!target)
    			return;
    		plunging=true;
    	}
    void InitiateBulletShower()
    	{
    		if(!target)
    			return;
    		doingBulletShower=true;
    	}
    void DoPlunge()
    	{
    		
    		
    		if(!plungePhase1Completed)
    			{
    				if(!plungeDirectionDecided)
    					{
    						plungeDirectionDecided=true;
    						rb.velocity=Quaternion.Euler(0,0,270) * transform.rotation * Vector2.right * plungeSpeed;

    					}
    				plungeCounter+= Time.deltaTime;
    				if(plungeCounter > maxPlungeTimePhase1)
    					{

    						plungePhase1Completed=true;
    						plungeDirectionDecided=false;
    						plungeCounter=0f;
    						rb.velocity= Vector2.zero;
    					}

    			}
    		else if(!plungePhase2Completed)
    			{
    				if(!plungeDirectionDecided)
    					{
    						FocusOnTarget(true);
    						plungePhase1Completed=true;
    						rb.velocity=Vector2.zero;
    						
    					}
    				plungeCounter+= Time.deltaTime;
    				if(plungeCounter > maxPlungeTimePhase2)
    					{
    						plungePhase2Completed=true;
    						plungeCounter=0f;    						
    						plungeDirectionDecided=false;
    						rb.velocity=Quaternion.Euler(0,0,90) * transform.rotation * Vector2.right * plungeSpeed;
    					}

    			}
    		else
    			{
    				plungeCounter+= Time.deltaTime;
    				if(plungeCounter > maxPlungeTimePhase3)
    					{
    						rb.velocity=Vector2.zero;
    						plunging=false;
    						plungeCounter=0f;
    						plungeDirectionDecided=false;
    						plungePhase2Completed=false;
    						plungePhase1Completed=false;
    					}

    			}

    	}
    void OnCollisionEnter2D(Collision2D col)
    	{
    		if(col.gameObject.tag == enemyString)
    			{
    				if(plunging && plungePhase2Completed)
    					{
    						rb.velocity=Vector2.zero;
    						plunging=false;
    						plungeCounter=0f;
    						plungeDirectionDecided=false;
    						plungePhase2Completed=false;
    						plungePhase1Completed=false;	

    					if(!col.gameObject.GetComponent<WrapperControl>().IsAI)
						  	col.gameObject.GetComponent<WrapperControl>().TakeDamage(plungeDamage);
						else 
						  	col.gameObject.GetComponent<WrapperControl>().TakeDamageAI(plungeDamage);

    					}
    			}
    	}

    void StayAndShoot()
    	{

    		plungeCounter+=Time.deltaTime;
    		if(plungeCounter > ssShootTime)
    			{
    				stayingAndShooting=false;
    				plungeCounter=0f;
    				attackCounter=maxAttackCounterTime;
    				return;
    			}
    		attackCounter+=Time.deltaTime;
    		FocusOnTarget();
    		if(attackCounter > maxAttackCounterTime)
    			{
    				attackCounter=0f;
    				Shoot();
    			}

    	}

	void Shoot()
		{
			GameObject gO= Instantiate(bulletx,bulletSpawnPos.transform.position,Quaternion.identity);	
			gO.SendMessage("GetRotation",transform.rotation);
			gO.SendMessage("GetParentBug",gameObject);
		}
	void WatchOutForCloseByEnemy()
		{
			if(!target) return;
			if(Vector2.Distance(transform.position,target.transform.position) > enemyProxForFireShower)
				{
					watchOutClock=Time.time;
				}
			else
				{
					if(Time.time-watchOutClock > maxWatchOutClockTime)
						{
							FireShower();
							watchOutClock=Time.time;
						}
				}
		}
	void WatchOutForFarByEnemy()
		{
			
			if(!target) return;
			if(Vector2.Distance(transform.position,target.transform.position) < minGapWithTarget)
				{
					watchOutClock_far=Time.time;
				}
			else
				{
					if(Time.time-watchOutClock_far > maxWatchOutClockTime_far)
						{
							FireShower();
							watchOutClock_far=Time.time;
						}
				}
		}


	void FireShower()
		{			
			for(int i=0;i < maxFires; i++)
				{
					GameObject gO= Instantiate(fireProjectile,
											   transform.position,
											   Quaternion.identity);
					gO.SendMessage("GetOutwardAngle",i*Mathf.Round(360/maxFires));
					gO.SendMessage("GetParent",gameObject);
				}
		}

	void ScanArea()
		{
			ContactFilter2D cf=new ContactFilter2D();
			Collider2D[] results=new Collider2D[10];
			 Physics2D.OverlapCircle(transform.position,50f,cf,results);
			 // Debug.Log(results[0]);
			 foreach(Collider2D col in results)
			 	{
			 		if(col)
			 			{
			 				Debug.Log(col.gameObject.tag);
			 				if(col.gameObject.tag==enemyString)
			 					{
			 						target=col.gameObject;
			 						// Debug.Log("Enemy is in");
			 						return;
			 					}
			 			}
			 		
			 	}
		}

	void FocusOnTarget(bool x=false)
			{
				float tspeed=0f;
				if(!x)
					tspeed=turnSpeed;
				else
					tspeed=quickFocus;

				if(target)
				transform.rotation=Quaternion.Slerp(transform.rotation,
					                               getRotationAngle(transform.position,target.transform.position),
					                                tspeed*Time.deltaTime
					                            	);
				
				
				
			}
	bool TimeForRandomBulletShower()
		{
			attackIdleTimer+=Time.deltaTime;

			if(attackIdleTimer >  maxTimeElpasedIdle)
				{
					rb.velocity= Vector2.zero;
					int random_attack_index=Random.Range(0,3);
					if(random_attack_index == 0)					
					// InitiateBulletShower();
						InitiatePlunge();
					else if(random_attack_index == 1)
						InitiateBulletShower();
					else
					// InitiateBulletShower();
						InitiateStayAndShoot();

					attackIdleTimer=0f;
					return true;
				}
			return false;
		}

	void MoveTowardsTarget()
		{
			if(!target) return;
			if(TimeForRandomBulletShower())
				return;
			if(IsFacingTarget() && Vector2.Distance(target.transform.position,transform.position) < minGapWithTarget)
				{
					rb.velocity=Vector2.zero;
					LaunchARandomAttack();
					attackIdleTimer=0f;
					return;
				}
			rb.velocity=Quaternion.Euler(0,0,90) * transform.rotation * Vector2.right * bugSpeed;
		}
	public bool IsInHornRange(GameObject targetx)
		{
			Vector2 targetDir=targetx.transform.position-transform.position;
			targetDir=targetDir.normalized;
			Vector2 facingDir =Quaternion.Euler(0,0,90)* transform.rotation * Vector2.right;
			facingDir=facingDir.normalized;
			if(Vector2.Dot(facingDir,targetDir) > 0.9f) return true;
			return false;
		}
	bool IsFacingTarget()
		{
			Vector2 targetDir=target.transform.position-transform.position;
			targetDir=targetDir.normalized;
			Vector2 facingDir =Quaternion.Euler(0,0,90)* transform.rotation * Vector2.right;
			facingDir=facingDir.normalized;
			if(Vector2.Dot(facingDir,targetDir) > 0.95f) return true;
			return false;
		}
	Quaternion getRotationAngle(Vector3 src,Vector3 tgt)
		{
			float angle=Mathf.Atan2((tgt.y-src.y),(tgt.x-src.x)) * Mathf.Rad2Deg;			
			return Quaternion.Euler(0,0,angle-90);
		}

    //bug's public attack information
	public bool IsAttacking
		{
			get { return stayingAndShooting || doingBulletShower || plunging;}
		}
	public bool DoingBulletShower_info
		{
			get {return doingBulletShower;}
		}
	//bug's public attack information
	void LaunchARandomAttack()
		{
			if(stayingAndShooting || doingBulletShower || plunging)
				{
					return;
				}
			int rv=Random.Range(0,2);
			if(rv == 0)
				{
					InitiateStayAndShoot();
				}
			else 
				{
					InitiatePlunge();
				}
			// else
			// 	{
			// 		InitiateBulletShower();
			// 	}
		}
	void DoBulletShower()
		{

			if(!poofed)
				{
					Instantiate(cloudPoof,poofPosition.transform.position,Quaternion.identity);
					poofed=true;
				}
			attackStartTimer+=Time.deltaTime;
			if(attackStartTimer < maxAttactStartTimer)
				{
					return;
				}
			FocusOnTarget();
			if(!alignmentReadyForShower && IsFacingTarget())
				{
					alignmentReadyForShower=true;
				}
			if(!alignmentReadyForShower)
				{
				bulletTimerWaitTime+=Time.deltaTime;
				if(bulletTimerWaitTime > maxBulletTimerWaitTime)
					{
						bulletTimerWaitTime=0f;
						alignmentReadyForShower=true;
					}
				else					
					return;
				}


			bulletShowerTime+=Time.deltaTime;
			if(bulletShowerTime > maxBulletShowerTime)
				{
					bulletShowerTime=0f;
					doingBulletShower=false;
					presentAngleDiff=0f;
					poofed=false;
					attackStartTimer=0f;
					alignmentReadyForShower=false;
					return;
				}
			rb.velocity=Vector2.zero;
			transform.rotation= Quaternion.Euler(0,0,presentAngleDiff)*transform.rotation;
			if ((angleInterval > 0 && presentAngleDiff > maxHalfAngle) 
				||
				(angleInterval < 0 && presentAngleDiff < -maxHalfAngle)
			   )
				{				
					angleInterval*=-1;
				}
			presentAngleDiff+=angleInterval;

			quickBulletTimer+=Time.deltaTime;
			if(quickBulletTimer > quickBulletGap)
				{
					quickBulletTimer=0f;
					Shoot();
				}

		}

	}
