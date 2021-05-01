using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrapperControl : MonoBehaviour 
	{

	Rigidbody2D rb;
	public float rotationSpeed=5f,rayCastDistance=10f;
	public float playerSpeed=8f;
	public TorsoControl tc_Script;
	public FeetControl fc_Script;
	bool doingSpecialAttack1,dodging,startedDodging,doingMeleeAttack;
	bool phaseOneComplete,phaseTwoComplete,phaseThreeComplete;
	float attackCounter1;
	public float specAttackPhaseOneTime=1.5f;
	public float specAttackPhaseTwoTime=1f;
	public float specAttackPhaseThreeTime=1.5f;
	public float plungeAttackSpeed=15f;
	public float dodgeSpeed=10f;
	bool dodgePhase1Complete;
	public float dodgeTime=0.5f;
	float dodgeTimer=0.0f;

	GameObject enemy;
	public bool isAI;
	public float turnSpeed=5f;
	public string enemyString="Enemy";
	public float proximDistance=2f;

	float aiAttackTimer=0f;
	public float maxAIAttackTime=2f;
	float health=0f;
	public float maxHealth=100f,staggerTimer=0f;
	public GameObject redBarAI,greenBarAI;

	bool dodged_ai;
	bool evading,phase1evade,staggering;
	float evadeTimer;
	public float evadeMaxTime=4f;
	public float waitAfterEvadeTime=2f;
	public float reasonableGapDistance=4f;
	public int dodgeAfterN=2;
	int atkCount=0;
	public float maxStaggerTime=2f;
	public float stabDamage=5f;
	public float plungeDamage=20f;
	public bool staggerEnabled,dodgingDisabled;
	public GameObject cloudPoof;

	float lastEvadeTimePoint=0f;
	public float maxEvasionManGap=5f;

	string enemyIsa="noman";
	float escapeTimer=0f;
	public float maxEscapeTime=4f;

	int debugCounter=0;
	public float verticalOffset=3f;
	public float raycastLengthFront=5f;

	//corner Escape parameters starts
	bool doingCornerAvoidance,cornerEscapeDirFixed;
	Quaternion cornerEscapeDir;
	float cornerEscapeTimer;
	public float maxCornerEscapeTimer=3f;
	//corner Escape parameters ends

	float cornerClock=0f;
	public float maxTimeUntilNextCornerEvasion=3f;
	float cWaitClock=0f;
	public float maxCWaitClock=3f;
	float obliqueEscapeAngle=135,obliqueEscapeAngle_to=80;
	bool adjustedEscapeAngle=false;
	public float longRayDist=100f;

	bool revealed=true;
	bool freezeAll;
	Collider2D ourCollider;
	public float iFrameTime=1f;

	public bool IsConspicous
		{
			get {return revealed;}
		}
	void Displayx(string x)
		{
			debugCounter+=1;
			Debug.Log(debugCounter+ ": "+x);
		}

	bool CanDoCornerEvasion()
		{
			return (Time.time - cornerClock > maxTimeUntilNextCornerEvasion);
		}

	public bool IsAttacking
		{
			get {return doingMeleeAttack; }
		}
	public bool IsAI
		{
			get {return isAI;}
		}
	void DodgeIfPossible()
		{
			if(!isAI || dodgingDisabled)
				return;
			if(enemy && enemy.GetComponent<SelfIDscript>().strid=="Soldier" )
			{
			if(EnemyAtProximDistance())
				{
					if(enemy.GetComponent<WrapperControl>().IsAttacking && 
						enemy.GetComponent<WrapperControl>().CheckHitTargetsWithName(gameObject.name)
					  )
						{
							DoDodge();
						}
				}
			}
			else if(enemy && enemy.GetComponent<SelfIDscript>().strid=="Bug")
			{
			if(EnemyAtProximDistance())
				{
					if(enemy.GetComponent<BugWrapper>().IsAttacking && 
						enemy.GetComponent<BugWrapper>().CheckHitTargetsWithName(gameObject.name)
					  )
						{
							DoDodge();
						}
				}
			}
		}
	void PerformDisappearingDodge()
		{

			/*
			if(revealed)
			{
				revealed=false;
				tc_Script.TurnOffRendering();
				fc_Script.TurnOffRendering();
				ourCollider.isTrigger=true;
			}
			else
			{
				revealed=true;
				tc_Script.TurnOnRendering();
				fc_Script.TurnOnRendering();
				ourCollider.isTrigger=false;
			}
			*/
			if(revealed)
			{
				revealed=false;
				tc_Script.TurnOffRendering();
				fc_Script.TurnOffRendering();
				ourCollider.isTrigger=true;
				StartCoroutine(TurnBackOnAfterNSeconds());
			}



		}
	IEnumerator TurnBackOnAfterNSeconds()
		{
			
			yield return new WaitForSeconds(iFrameTime);
			if(!revealed)
				{
				revealed=true;
				tc_Script.TurnOnRendering();
				fc_Script.TurnOnRendering();
				ourCollider.isTrigger=false;
				}			
		}
	void Stagger()
		{
			if(!staggerEnabled)
				{
				staggering=false; 
				return;
				}
			rb.velocity=Vector2.zero;
			staggerTimer+=Time.deltaTime;
			if(staggerTimer > maxStaggerTime)
				{
					staggering=false;
					staggerTimer=0f;
				}
		}

    public void TakeDamage(float dmg,bool heavy=true)
    	{
    		health-=dmg;
    		if(heavy)
    			staggering=true;
    		if(!isAI)
    			{
    			GameObject.Find("HealthBar").GetComponent<Image>().fillAmount=health/maxHealth;	
    			}
    		
    		if(health <= 0)
    			{
    				Debug.Log("Player has died");
    			}
    	}
	public void TakeDamageAI(float dmg,bool heavy=true)
		{
			// Debug.Log("damage received");
			health-=dmg;
			redBarAI.gameObject.SetActive(true);
			greenBarAI.gameObject.SetActive(true);			
			if(health>=0)
				greenBarAI.GetComponent<Image>().fillAmount=health/maxHealth;
			StartCoroutine(hideHealthBar());
			//insert stagger script here
			if(health < 0)
				{
					
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

	void InitiateCornerAvoidance()
		{
			Displayx("Starting CA");
			doingCornerAvoidance=true;
		}


	void DoCornerAvoidance()
		{
			if(!doingCornerAvoidance) return;
			cWaitClock=0f;
			if(!cornerEscapeDirFixed)
				{
					cornerEscapeDir=FindExit()*transform.rotation;
					cornerEscapeDirFixed=true;
				}
			transform.rotation=Quaternion.Lerp(transform.rotation,
								 			   cornerEscapeDir,
								 			   10f
				                              );
			MoveForeward();

			cornerEscapeTimer+=Time.deltaTime;
			if(cornerEscapeTimer > maxCornerEscapeTimer)
				{
					// cornerEscapeTimer=0f;
					// doingCornerAvoidance=false;
					// cornerEscapeDirFixed=false;
					// rb.velocity=Vector2.zero;
					// cornerClock=Time.time;
					ResetAfterCornerAvoidance();
					Displayx("Corner avoid ends");
					// freezeAll=true; //kmherereversh
				}

		}

	void ResetAfterCornerAvoidance()
		{
			cornerEscapeTimer=0f;
			doingCornerAvoidance=false;
			cornerEscapeDirFixed=false;
			rb.velocity=Vector2.zero;
			cornerClock=Time.time;
			// cWaitClock=0f;

		}



	void Start () 
		{
		 ourCollider=GetComponent<Collider2D>();
		 rb=GetComponent<Rigidbody2D>();
		 health=maxHealth;

		 // InitiateCornerAvoidance();
		 // cornerEscapeDir=FindExit();
		 
		
		}
	
	
	void Update () 
		{

			//AdjustEscapeAngle();
			//return; This is working perfectly
		if(freezeAll) return;

		
		if(isAI && !enemy)
			ScanArea();
		
		float horizInput=-1*Input.GetAxis("Horizontal");
		float vertInput = Input.GetAxis("Vertical");		

		if(doingSpecialAttack1)
			SpecialAttack1();
		else if(dodging)
			Dodge();
		else if(doingMeleeAttack)
			rb.velocity=Vector2.zero;
		else if(evading)
			Evade();
		else if(staggering)
			Stagger();
		else if(doingCornerAvoidance)
			DoCornerAvoidance();
		else if(!isAI)
			{
			  tc_Script.MoveOrStop(vertInput);
			  fc_Script.MoveOrStop(vertInput);
			  transform.rotation=Quaternion.Euler(0,0,horizInput * rotationSpeed) * transform.rotation;
			  rb.velocity=transform.rotation * Vector2.right * playerSpeed * vertInput;

			  if(Input.GetButtonDown("Jump"))
				PerformDisappearingDodge();//DoDodge();	
			  else if(Input.GetKeyDown(KeyCode.Keypad0))
			  	DoMeleeAttack();

			}
		else if(isAI)
			{
				PeriodicCornerWaitCheck();
				if(enemyIsa=="Soldier")
					 BaseAIBehaviour();
				else if(enemyIsa=="Bug")
					AIattackBehaviour();
			}
		

		// CheckForHitVictims();
		
		}

   void BaseAIBehaviour()
   		{	
   			if(!enemy)
   				{
   					rb.velocity=Vector2.zero;
   					return;
   				}
			DodgeIfPossible();
			float isMoving=0f;
			SlowlyTurnTowardsEnemy();
			if(!EnemyAtProximDistance())
				{
				MoveForeward();
				isMoving=1f;
				}
			else
				{
				rb.velocity=Vector2.zero;
				isMoving=0f;
				if(IsTimeToAttack())
					{
						InitiateRandomAttack(3);
					}
				}
				tc_Script.MoveOrStop(isMoving);
				fc_Script.MoveOrStop(isMoving);
   		}
   bool IsInDanger()
   	{
   		if(enemy.GetComponent<BugWrapper>().IsAttacking &&
   			enemy.GetComponent<BugWrapper>().IsInHornRange(gameObject))
   		return true;
   		return false;
   	}
   void InitiateEscape()
   		{
   			escapeTimer+=Time.deltaTime;
   		}
   bool IsEscaping()
   		{
   			if(escapeTimer==0f)
   				return false;
   			else
   				{
   					escapeTimer+=Time.deltaTime;
   					if(escapeTimer > maxEscapeTime)
   						{
   							escapeTimer=0f;   							
   						}
   					return true;
   				}
   		}
Quaternion FindExit()
		{
			
			float angleToCenter=ClearPathToCenter();
			if(angleToCenter!=0)
				{
					// Displayx("CLear path exists:"+angleToCenter);
					return Quaternion.Inverse(transform.rotation)*Quaternion.Euler(0,0,angleToCenter);
				}

			for(float i=0f;i < 360f;i+=1f)
				{
					if(!DetectObjectFront(i))
						return Quaternion.Euler(0,0,i);
				}
			return  Quaternion.Euler(0,0,270);//kmhere this needs review
		}


//kmhere functionality to change escape angle starts
void AdjustEscapeAngle()
	{
		string[] tags=new string[]{"Pipe"};
		float enemyangSep=getRotationAngle_angle(transform.position,enemy.transform.position);
		if(DetectWallOnAnyDir( enemyangSep+obliqueEscapeAngle,tags,true))
			{
				obliqueEscapeAngle*=-1;

			}
		if(DetectWallOnAnyDir(enemyangSep+obliqueEscapeAngle_to,tags,true))
			{
				obliqueEscapeAngle_to*=-1;
				// Displayx("eighty changed to:"+obliqueEscapeAngle_to);
			}
	}
float ClearPathToCenter()
	{
		GameObject center=GameObject.Find("CenterPoint");
		// Displayx(center.transform.position.x+","+center.transform.position.y);
		string[] tagsx=new string[]{"Bug"};
		if ( DetectWallOnAnyDir(getRotationAngle_angle(transform.position,center.transform.position),tagsx) )
			return 0f;
		else
			return getRotationAngle_angle(transform.position,center.transform.position);
		  
	}
bool DetectWallOnAnyDir(float checkAngle,string[] tagsrec,bool useShortRay=false)
		{

			// string[] tagsx=new string[]{"Pipe"};
			float usedRayDist;
			if(useShortRay)
				usedRayDist=2*rayCastDistance;
			else
				usedRayDist=longRayDist;
			
			bool hit1=detectObjectsWithTag(transform.position+Quaternion.Euler(0,0,90)*
								 Quaternion.Euler(0,0,checkAngle)*
								 Vector2.right*verticalOffset,
							          Quaternion.Euler(0,0,checkAngle)*Vector2.right*usedRayDist,
							          tagsrec);
			bool hit2=detectObjectsWithTag(transform.position,
					                  Quaternion.Euler(0,0,checkAngle)*Vector2.right*usedRayDist,
							          tagsrec);
			bool hit3=detectObjectsWithTag(transform.position-Quaternion.Euler(0,0,90)*
								 Quaternion.Euler(0,0,checkAngle)*
								 Vector2.right*verticalOffset,
							          Quaternion.Euler(0,0,checkAngle)*Vector2.right*usedRayDist,
							          tagsrec);
			Debug.DrawRay(transform.position,
				          Quaternion.Euler(0,0,checkAngle)*Vector2.right*usedRayDist,
				          Color.red);	

			
			return hit1||hit2||hit3 ;
		}
// functionality to change escape angle ends
bool DetectObjectFront(float angleOffset=0f)
		{
			Quaternion aoq=Quaternion.Euler(0,0,angleOffset);
			Debug.DrawRay(transform.position-Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*verticalOffset,
				          aoq*transform.rotation*Vector2.right*raycastLengthFront,
				          Color.red);
			Debug.DrawRay(transform.position,
				          aoq*transform.rotation*Vector2.right*raycastLengthFront,
				          Color.red);
			Debug.DrawRay(transform.position+Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*verticalOffset,
				          aoq*transform.rotation*Vector2.right*raycastLengthFront,
				          Color.red);
			

			string[] tags=new string[]{"Pipe",enemyString};

			bool hit1=detectObjectsWithTag(transform.position-Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*verticalOffset,
							          aoq*transform.rotation*Vector2.right*raycastLengthFront,
							          tags);			
			bool hit2=detectObjectsWithTag(transform.position,
				          aoq*transform.rotation*Vector2.right*raycastLengthFront,
				          tags);			
			bool hit3=detectObjectsWithTag(transform.position+Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*verticalOffset,
				          aoq*transform.rotation*Vector2.right*raycastLengthFront,
				          tags);
			return hit1 || hit2 || hit3;
		}
bool detectObjectWithTag(Vector2 origin,Vector2 dir,string t_g)
        {
            
            RaycastHit2D[] hits=Physics2D.RaycastAll(origin,dir.normalized,dir.magnitude);
            
            foreach(RaycastHit2D hitx in hits)
                {
                    if(hitx.collider.tag==t_g)
                        return true;
                }
            return false;
        }
bool detectObjectsWithTag(Vector2 origin,Vector2 dir,string[] t_gs)
        {
            
            RaycastHit2D[] hits=Physics2D.RaycastAll(origin,dir.normalized,dir.magnitude);
            
            foreach(RaycastHit2D hitx in hits)
                {
                	foreach(string t_g in t_gs)
                    if(hitx.collider.tag==t_g)
                        return true;
                }
            return false;
        }
   void AIattackBehaviour()
   		{	
   		if(!enemy)
   				{
   					rb.velocity=Vector2.zero;
   					return;
   				}		
		float isMoving=0f;
		bool isEscaping= IsEscaping();
		
		if(isEscaping)
			{
				// debugCounter+=1;
				// Debug.Log(debugCounter+": Iam escaping");
			}
		// isEscaping=true;
		if(IsInDanger())//kmherereverseh (IsInDanger())
			{
			//added 24_4_21 starts
			if(!adjustedEscapeAngle)
				{
					adjustedEscapeAngle=true;
					AdjustEscapeAngle();
				}
			//added 24_4_21 ends
			ObliquelyTurnTowardsEnemy(isEscaping);

			}
		else
			{
			//added 24_4_21 starts
			if(adjustedEscapeAngle) adjustedEscapeAngle=false;
			//added 24_4_21 ends
			SlowlyTurnTowardsEnemy(1,isEscaping);
			}
		if(!EnemyAtProximDistance() || isEscaping)
			{
			MoveForeward();
			isMoving=1f;
			}
		else
			{
			if(enemy.GetComponent<BugWrapper>().IsAttacking)
				{
				// Debug.Log("bug is attacking ..");
				if(enemy.GetComponent<BugWrapper>().DoingBulletShower_info || 
					enemy.GetComponent<BugWrapper>().IsInHornRange(gameObject)
				   )		
				InitiateEscape();
				}
			else
				{
				SlowlyTurnTowardsEnemy(6);
				rb.velocity=Vector2.zero;
				isMoving=0f;
				if(IsTimeToAttack())
					{
						InitiateRandomAttack(3,false);
					}
				}
				tc_Script.MoveOrStop(isMoving);
				fc_Script.MoveOrStop(isMoving);
			}
	
   		}
   	void ObliquelyTurnTowardsEnemy(bool away=false)
   		{	
   		if(!enemy)
				return;	
	
		Quaternion enemyAngularLocation=getRotationAngle(transform.position,
																enemy.transform.position);;
		if(!away)
		 	enemyAngularLocation=Quaternion.Euler(0,0,obliqueEscapeAngle_to)*enemyAngularLocation;
		else
			enemyAngularLocation=Quaternion.Euler(0,0,obliqueEscapeAngle)*enemyAngularLocation;

		 
		 transform.rotation=Quaternion.Lerp(transform.rotation,
		 									enemyAngularLocation,
		 									Time.deltaTime * turnSpeed);
		 
   		}
   



	void SlowlyTurnTowardsEnemy(int n=1,bool away=false)
		{
			if(!enemy)
				return;

			Quaternion enemyAngularLocation=getRotationAngle(transform.position,enemy.transform.position);
			if(away)
				enemyAngularLocation=Quaternion.Euler(0,0,180)*enemyAngularLocation;
			
			transform.rotation=Quaternion.Lerp(transform.rotation,
												enemyAngularLocation,
												Time.deltaTime * turnSpeed*n);


		}

	void Evade()
		{
			if(!evading)
				return;
			if(!phase1evade)
				{   
					SlowlyTurnAwayFromEnemy();
					MoveForeward();
					evadeTimer+=Time.deltaTime;
					if(evadeTimer > evadeMaxTime)
						{
							evadeTimer=0f;
							phase1evade=true;
						}
				}
			else
				{
					rb.velocity=Vector2.zero;
					SlowlyTurnTowardsEnemy();
					evadeTimer+=Time.deltaTime;
					if(evadeTimer > waitAfterEvadeTime || EnemyClosedGap())
						{
							evadeTimer=0f;
							phase1evade=false;
							evading=false;
						}
				}
		}
	void SlowlyTurnAwayFromEnemy()
		{
			if(!enemy)
				return;
			Quaternion enemyAngularLocation=getRotationAngle(transform.position,enemy.transform.position);
			enemyAngularLocation= Quaternion.Euler(0,0,180) * enemyAngularLocation;
			transform.rotation=Quaternion.Lerp(transform.rotation,enemyAngularLocation,Time.deltaTime * turnSpeed);
		}

	public void ComeOutOfAttack()
		{
			doingMeleeAttack=false;
		}

	void TurnTowardsEnemy()
		{
			if(!enemy)
				return;
			transform.rotation=getRotationAngle(transform.position,enemy.transform.position);
		}
	void MoveForeward()
		{
			rb.velocity=transform.rotation * Vector2.right * playerSpeed;  
			
		}
	bool EnemyAtProximDistance()
		{
			if(!enemy) return false;
			Debug.DrawRay(transform.position,
					      transform.rotation * Vector2.right*proximDistance,
					      Color.red); 
			if(Vector2.Distance(transform.position,enemy.transform.position)< proximDistance)
				return true;
			return false;
		}
	bool EnemyClosedGap()
		{
			if(Vector2.Distance(transform.position,enemy.transform.position)< reasonableGapDistance)
				return true;
			return false;
		}

	Quaternion getRotationAngle(Vector3 src,Vector3 tgt)
		{
			float angle=Mathf.Atan2((tgt.y-src.y),(tgt.x-src.x)) * Mathf.Rad2Deg;			
			return Quaternion.Euler(0,0,angle);
		}
	float getRotationAngle_angle(Vector3 src,Vector3 tgt)
		{
			float angle=Mathf.Atan2((tgt.y-src.y),(tgt.x-src.x)) * Mathf.Rad2Deg;			
			return angle;
		}

	//AI Attacks
	void InitiateRandomAttack(int x,bool retreatAllowed=true)
		{
			/*
			atkCount+=1;
			if(atkCount >= dodgeAfterN)
				{
					DoEvade();
					atkCount=0;
					return;
				}
			*/
			bool clearToEvade=true;
			if(enemy.gameObject.GetComponent<SelfIDscript>().strid=="Bug")
				{
					if(enemy.gameObject.GetComponent<BugWrapper>().DoingBulletShower_info)
						clearToEvade=false;
				}

			if(retreatAllowed)
				{
				if(Time.time - lastEvadeTimePoint > maxEvasionManGap && clearToEvade)
					{
						lastEvadeTimePoint=Time.time;
						DoEvade();
						return;
					}
				}

			int choice=Random.Range(1,x);
			if(choice==1)
				{
					DoMeleeAttack();
				}
			else 
				{
					DoSpecialAttack1();
				}

		}
	bool IsTimeToAttack()
		{
			aiAttackTimer+=Time.deltaTime;
			if(aiAttackTimer >= maxAIAttackTime)
				{
					aiAttackTimer=0f;
					return true;
				}
			return false;
		}
	void DoMeleeAttack()
		{
			if(!doingMeleeAttack)
				{
					tc_Script.Attack();
				}
			doingMeleeAttack=true;

		}
	void DoDodge()
		{
			dodging=true;
			
		}
	void DoEvade()
		{
			evading=true;
		}
	void DoSpecialAttack1()
		{
			
			doingSpecialAttack1=true;
		}
	//AI Attacks

	void Dodge()
		{
			if(!dodging)
				return;
			Vector2 dir;
			dir=Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*dodgeSpeed;
			// if(!startedDodging)
			// 	{
			// 		startedDodging=true;
			// 		dir=Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*dodgeSpeed;
			// 	}
			if(!dodgePhase1Complete) 
				{
					rb.velocity=dir;
					dodgeTimer+=Time.deltaTime;
					if(dodgeTimer >= dodgeTime)
						{
							dodgeTimer=0f;
							dodgePhase1Complete=true;
						}
				}
			/*
			else
				{
					rb.velocity=-dir;
					dodgeTimer+=Time.deltaTime;
					if(dodgeTimer >= dodgeTime)
						{
							dodgeTimer=0f;
							startedDodging=false;
							dodging=false;
							dodgePhase1Complete=false;
							rb.velocity=Vector2.zero;
						}
				}
			*/
		    else
		    	{

				startedDodging=false;
				dodging=false;
				dodgePhase1Complete=false;
				rb.velocity=Vector2.zero;
		    	}

		}
	void SpecialAttack1()
		{
			if(!doingSpecialAttack1)
				return;
			Vector2 calcDir=transform.rotation * Vector2.right;
			if(!phaseOneComplete)
				{
					attackCounter1+=Time.deltaTime;
					rb.velocity=transform.rotation * Vector2.right * plungeAttackSpeed * -1;
					if(attackCounter1 > specAttackPhaseOneTime)
						{
							attackCounter1=0f;
							phaseOneComplete=true;
						}
				}
			else if(!phaseTwoComplete)
				{
					if(attackCounter1==0f )
						{
							TurnTowardsEnemy();
							tc_Script.Attack();
						}
					attackCounter1+=Time.deltaTime;
					rb.velocity=Vector2.zero;
					if(attackCounter1 > specAttackPhaseTwoTime)
						{
							attackCounter1=0f;
							phaseTwoComplete=true;
						}
				}
			else if(!phaseThreeComplete)
				{
					attackCounter1+=Time.deltaTime;
					rb.velocity=calcDir * plungeAttackSpeed ;
					if(attackCounter1 > specAttackPhaseThreeTime)
						{
							attackCounter1=0f;
							phaseThreeComplete=true;
						}
				}
			else
				{
					phaseOneComplete=false;
					phaseTwoComplete=false;
					phaseThreeComplete=false;
					doingSpecialAttack1=false;
					//attackCounter1=0f;//unnecessary here
				}
		}

	void OnCollisionEnter2D(Collision2D col)
		{
			if(col.gameObject.tag==enemyString)
				{
				if(col.gameObject.GetComponent<SelfIDscript>().strid=="Soldier")
					{
					if(doingSpecialAttack1 && phaseOneComplete && phaseTwoComplete)
						{
						  phaseOneComplete=false;
						  phaseTwoComplete=false;
						  phaseThreeComplete=false;
						  doingSpecialAttack1=false;
						  attackCounter1=0f;
						  //Do Damage to enemy
						  if(!enemy.GetComponent<WrapperControl>().IsAI)
						  	col.gameObject.GetComponent<WrapperControl>().TakeDamage(plungeDamage);	
						  else 
						  	col.gameObject.GetComponent<WrapperControl>().TakeDamageAI(plungeDamage);	
						}
					}
				else if(col.gameObject.GetComponent<SelfIDscript>().strid=="Bug")
					{
					if(doingSpecialAttack1 && phaseOneComplete && phaseTwoComplete)
						{
						  phaseOneComplete=false;
						  phaseTwoComplete=false;
						  phaseThreeComplete=false;
						  doingSpecialAttack1=false;
						  attackCounter1=0f;
						 
						  col.gameObject.GetComponent<BugWrapper>().TakeDamageAI(plungeDamage);	
						}	
					}

					
				}
			else if(col.gameObject.tag=="Pipe")
				{
					if(isAI && !doingCornerAvoidance )  //&& CanDoCornerEvasion() taken off
						cWaitClock=Time.time;// InitiateCornerAvoidance();
					else if(doingCornerAvoidance)
						ResetAfterCornerAvoidance();

				}
		}
	void OnCollisionExit2D(Collision2D col)
		{
			if(col.gameObject.tag=="Pipe")
				{
					cWaitClock=0f;
				}
		}
	void PeriodicCornerWaitCheck()
		{
			if(cWaitClock==0f) return;
			if(Time.time-cWaitClock > maxCWaitClock)
				InitiateCornerAvoidance();
		}
	void Attack()
		{
			tc_Script.Attack();
		}

	void ScanArea()
		{
			ContactFilter2D cf=new ContactFilter2D();
			Collider2D[] results=new Collider2D[10];
			 Physics2D.OverlapCircle(transform.position,10f,cf,results);
			 // Debug.Log(results[0]);
			 foreach(Collider2D col in results)
			 	{
			 		if(col)
			 			{
			 				if(col.gameObject.tag==enemyString)
			 					{
			 						enemy=col.gameObject;
			 						
			 						if(enemy.gameObject.GetComponent<SelfIDscript>().strid=="Bug")
			 							enemyIsa="Bug";
			 						else
			 							enemyIsa="Soldier";
			 						return;
			 					}
			 			}
			 		
			 	}
		}

	public bool CheckForHitVictims()
		{
			RaycastHit2D[] hits=Physics2D.RaycastAll(transform.position,
													 transform.rotation* Vector2.right,
													 rayCastDistance);
			Debug.DrawRay(transform.position,
				         transform.rotation* Vector2.right*rayCastDistance,
				          Color.red);
			foreach(RaycastHit2D hitx in hits)
                {
                	if(hitx.collider.tag==enemyString)
                		{
                			
                			if(hitx.collider.gameObject.GetComponent<SelfIDscript>().strid=="Soldier")                			
                				{
                				if(!hitx.collider.gameObject.GetComponent<WrapperControl>().IsAI)
                					{                				
                					hitx.collider.gameObject.GetComponent<WrapperControl>().TakeDamage(stabDamage);
                					}
                				else
                					{                				
                					hitx.collider.gameObject.GetComponent<WrapperControl>().TakeDamageAI(stabDamage);
                					}
                				}                			
                			else if(hitx.collider.gameObject.GetComponent<SelfIDscript>().strid=="Bug")
                				{								
                				hitx.collider.gameObject.GetComponent<BugWrapper>().TakeDamageAI(stabDamage);
                				}
                			
                		return true;    
                		}           	
                } 
            return false;
		}
	public bool CheckHitTargetsWithName(string nm)
		{
			RaycastHit2D[] hits=Physics2D.RaycastAll(transform.position,
													 transform.rotation* Vector2.right,
													 rayCastDistance);
			Debug.DrawRay(transform.position,
				         transform.rotation* Vector2.right*rayCastDistance,
				          Color.red);
			foreach(RaycastHit2D hitx in hits)
                {
                	if(hitx.collider.gameObject.name==nm)
                		{   
                		return true;    
                		}           	
                } 
            return false;
		}

	}
