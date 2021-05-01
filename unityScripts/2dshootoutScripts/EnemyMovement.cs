using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyMovement : MonoBehaviour {

	public GameObject shootTarget;
	public float rotationSpeed,playerSpeed;
	public GameObject p_feet,p_torso,bulletObj,bulletObjPos;
	public GameObject dodgePosition;
	Vector2 initialPosition;
	Rigidbody2D rb;
	bool freeToShoot,underTimer;
	string TARGET1="underRock",TARGET2="dodgePoint";
	string presentTarget;
	Vector2 targetPosition;
	public float minWaitTime=2f,maxWaitTime=4f,waitTime,waitTimer=0f;
	public float maxAttackGap=2f,directVisibilityProximDistance=100f;
	float attackGap=0f;

	
	public float maxHealth=100f;
	float health=0f;

	public GameObject redBar,greenBar;

	bool activateMe=true;
	public LayerMask playerLayer;

	void activateAction()
		{
			if(Physics2D.OverlapCircle(transform.position,18f,playerLayer))
                    {                               
                        if(!activateMe) 
                        	activateMe=true;                        
                    }
            else if(activateMe)
            	activateMe=false;
		}
	bool CheckIfPlayerNear()
		{
			if(Physics2D.OverlapCircle(transform.position,18f,playerLayer))
				return true;
			return false;
		}
	public void TakeDamage(float dmg)
		{
			health-=dmg;
			if(health <= 0)
				Destroy(gameObject);
			redBar.gameObject.SetActive(true);
			greenBar.gameObject.SetActive(true);
			// Debug.Log("Health:"+(health/maxHealth));
			if(health>=0)
				greenBar.GetComponent<Image>().fillAmount=health/maxHealth;

			StartCoroutine(hideHealthBar());
		}
	IEnumerator hideHealthBar()
		{
			yield return new WaitForSeconds(1.0f);
			greenBar.gameObject.SetActive(false);
			redBar.gameObject.SetActive(false);
		}

	void MoveTowardsTarget()
		{

			TurntowardsTarget(targetPosition);
			if(activateMe){
			Vector2 dir=new Vector2(targetPosition.x,targetPosition.y) - new Vector2(transform.position.x,transform.position.y);
			rb.velocity=dir.normalized * playerSpeed;
						  }
		}
	void CheckIfTargetReached()
		{
			if(presentTarget==TARGET2)
				{

					if(Vector2.Distance(transform.position,dodgePosition.transform.position)<0.2f)
						{
							// Debug.Log("Target reset to initial position");
							presentTarget=TARGET1;
							targetPosition=initialPosition;
							rb.velocity=Vector2.zero;
						}
				}
			else if(presentTarget==TARGET1)
				{
					if(Vector2.Distance(transform.position,initialPosition)<0.2f)
						{							
							activateAction();
							if(activateMe){
							presentTarget=TARGET2;
							targetPosition=dodgePosition.transform.position;							
							waitTimer=0.1f;
										}
							// underTimer=true;
							rb.velocity=Vector2.zero;
							

						}

				}
		}
	void RunTimer()
		{
			
			if(waitTimer!=0f)
				{ 
				underTimer=true;
				waitTimer+=Time.deltaTime;
				if(waitTimer > waitTime)
					{
						waitTime=Random.Range(minWaitTime,maxWaitTime);
						waitTimer=0f;
						underTimer=false;
					}
				}
		}
	void CheckIfFreeToShoot()
		{
			if(presentTarget==TARGET2 || IsBehindBush() || DetectIncomingBullets() ||!activateMe )
				{
					freeToShoot=false;
				}
			else
				{
					freeToShoot=true;
				}
		}

	bool DetectIncomingBullets()
    	{
    		// return false;
    		foreach(GameObject g in GameObject.FindGameObjectsWithTag("bullet"))
    			{

    				if(g.GetComponent<BulletScript>().Parentx==gameObject)
    					continue;
    				
    				Vector2 targetDir=transform.position-g.transform.position;
    				Vector2 bulletDir=g.GetComponent<BulletScript>().VelocityDir();
    				// Vector2 bulletDir=Vector2.right;
    				targetDir=targetDir.normalized;
    				if(Mathf.Abs(targetDir.x-bulletDir.x) < 0.05f && Mathf.Abs(targetDir.y-bulletDir.y) < 0.05f)
    					{
    					
    					return true;
    					}
    			}
    		return false;
    	}
    bool IsBehindBush()
    	{
    		if(detectDirectVisibility(transform.position,shootTarget.transform.position-transform.position,"Player"))
    			return false;
    		else
    			return true;
    	}
    void AttackPlayer()
    	{
    		if(attackGap >= maxAttackGap)
    			{
    				attackGap=0f;
    				Attack();
    			}
    		else
    			{
    				attackGap+=Time.deltaTime;
    			}
    	}
    void Attack()
		{
			// Debug.Log("Iam attacking");
			p_torso.GetComponent<PlayerTorsoMovement>().Attack();	
		}

	void Start () {
		redBar.gameObject.SetActive(false);
		greenBar.gameObject.SetActive(false);
		health=maxHealth;
		attackGap=maxAttackGap;
		presentTarget=TARGET2;
		targetPosition=dodgePosition.transform.position;
		initialPosition=transform.position;
		rb=GetComponent<Rigidbody2D>();
	}
	
	
	void Update () {
		// activateAction();
		
		if(!shootTarget) return;
		CheckIfFreeToShoot();		
		CheckIfTargetReached();
		RunTimer();
		if(!freeToShoot && !underTimer)
			{
				MoveTowardsTarget();
				if(attackGap!=maxAttackGap)
					attackGap=maxAttackGap;
			}
		else if(freeToShoot)// && !IsBehindBush())
			{
				rb.velocity=Vector2.zero;
				TurntowardsTarget(shootTarget.transform.position);
				AttackPlayer();
			}
		

		//animate torso and feet according to vertical input 
		p_feet.GetComponent<PlayerFeetMovement>().moveOrStop(rb.velocity.magnitude);
		p_torso.GetComponent<PlayerTorsoMovement>().moveOrStop(rb.velocity.magnitude);
	}

	 bool detectDirectVisibility(Vector2 origin,Vector2 dir,string t_g)
    	{
    		// Debug.DrawRay(origin,
				  //         dir.normalized*18f,
				  //         Color.red);
    		 RaycastHit2D[] hits=Physics2D.RaycastAll(origin,dir.normalized,directVisibilityProximDistance);
            float minDist=26000f,minDistTag=26001f;
            foreach(RaycastHit2D hitx in hits)
                {
                	if(hitx.collider.tag=="Enemy" || hitx.collider.tag=="bullet" || hitx.collider.tag=="zombie") continue;
                	if(minDist > hitx.distance) minDist=hitx.distance;
                    if(hitx.collider.tag==t_g)
                        if(minDistTag > hitx.distance) minDistTag=hitx.distance;
                }        
            if(minDistTag <= minDist)
            	{            	
            	return true;
            	}
           
            return false;
    	}

	Quaternion getRotationAngle(Vector3 src,Vector3 tgt)
        {

            float angle=Mathf.Atan2((tgt.y-src.y),(tgt.x-src.x)) * Mathf.Rad2Deg;           
            return Quaternion.Euler(0,0,angle);
        }
    void TurntowardsTarget(Vector2 tgt)
    	{

    		transform.rotation=Quaternion.Slerp(transform.rotation,
											    getRotationAngle(transform.position,tgt),
											    10f*Time.deltaTime
				         					   );
    	}

    public void SpawnBullet()
		{
			GameObject g=Instantiate(bulletObj,bulletObjPos.transform.position,transform.rotation);
			g.SendMessage("GetParent",gameObject);
			// g.SendMessage("getRotation",transform.rotation);
		}
    
}
