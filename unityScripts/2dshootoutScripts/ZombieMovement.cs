using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieMovement : MonoBehaviour {

	public GameObject target,body,cpoof;
	public float sidewayOffset=3f,raycastLengthFront=10f;
	public float frontBackOffset=3f,raycastLengthSideways=10f;
	public float playerProximDistance=1f,zombieSpeed=5f;
	public LayerMask playerLayer;
	public float maxHealth=100f;
	public bool isRightZombie;
	float health;
	Rigidbody2D rb;
	ZombieAnimations z_script;
	float hindranceTimer=0f;
	public GameObject redBar,greenBar;
	string[] tags=new string[]{"bush","gift","pipe"};
	string[] tags_player=new string[]{"Player"};
	Vector3 initPosition;
	bool goingBack=false;
	void Start () {
		initPosition=transform.position;
		Instantiate(cpoof,transform.position,Quaternion.identity);
		redBar.gameObject.SetActive(false);
		greenBar.gameObject.SetActive(false);
		rb=GetComponent<Rigidbody2D>();	
		z_script=body.GetComponent<ZombieAnimations>();
		health=maxHealth;
	}
	
	
	void Update () {
		
		if(!target) return;
		if(!GameObject.Find("GameManager").GetComponent<GameManager>().startGame) return;
		if(!isRightZombie)
			{
			if(target.transform.position.x > 15f && !goingBack)
			goingBack=true;		
			}
		else
			{
			if(target.transform.position.x < 15f && !goingBack)
			goingBack=true;	
			}
		
		if(hindranceTimer!=0f)
			{
				hindranceTimer+=Time.deltaTime;				
				if(hindranceTimer > 1f)
					hindranceTimer=0f;
			}
		bool hindrance;
		if(goingBack)
			hindrance=DetectFenceInDirection(initPosition-transform.position);
		else
			hindrance=DetectFenceInDirection(target.transform.position-transform.position);
		if(hindrance || hindranceTimer!=0f)
			{
				if(hindranceTimer==0f)
				hindranceTimer=0.1f;
				if(goingBack)
					StrafeAroundHindrance(initPosition);
				else
					StrafeAroundHindrance(target.transform.position);
			}
		else
			{
			if(goingBack)
				TurntowardsTarget(initPosition);
			else
				TurntowardsTarget(target.transform.position);
			}
		MoveForward();
	}
	public void TakeDamage(float dmg)
		{
			health-=dmg;
			redBar.gameObject.SetActive(true);
			greenBar.gameObject.SetActive(true);			
			if(health>=0)
				greenBar.GetComponent<Image>().fillAmount=health/maxHealth;
			StartCoroutine(hideHealthBar());
			if(health < 0)
				{
					Instantiate(cpoof,transform.position,Quaternion.identity);
					Destroy(gameObject);
				}
		}
	IEnumerator hideHealthBar()
		{
			yield return new WaitForSeconds(1.0f);
			greenBar.gameObject.SetActive(false);
			redBar.gameObject.SetActive(false);
		}
	public void InflictDamage()
		{
			if(DetectPlayerInDirection(target.transform.position-transform.position))
				target.GetComponent<PlayerMovement>().TakeDamage(20f);
		}

	Quaternion getRotationAngle(Vector3 src,Vector3 tgt)
        {
            float angle=Mathf.Atan2((tgt.y-src.y),(tgt.x-src.x)) * Mathf.Rad2Deg;           
            return Quaternion.Euler(0,0,angle);
        }
    bool ReachedInitPosition()
    	{
    		if(Vector2.Distance(transform.position,initPosition)<1f)
    			return true;
    		return false;
    	}
    void MoveForward()
    	{
    		if(goingBack && ReachedInitPosition())
    			{
    			rb.velocity=Vector2.zero;
    			if(
    				(target && target.transform.position.x < 15f && !isRightZombie)
    				||
    				(target && target.transform.position.x > 15f && isRightZombie)
    			  )
    				goingBack=false;
    			}
    		else if(!goingBack && IsPlayerNear())
    		{
    		rb.velocity=Vector2.zero;
    		z_script.Attack();
    		}
    		else
    		rb.velocity=transform.rotation*Vector2.right*zombieSpeed;
    		z_script.Move(rb.velocity);
    	}

    bool IsPlayerNear()
    	{
    		if(Physics2D.OverlapCircle(transform.position,playerProximDistance,playerLayer))
    			return true;
    		return false;
    	}
    void TurntowardsTarget(Vector2 tgt)
    	{

    		transform.rotation=Quaternion.Slerp(transform.rotation,
											    getRotationAngle(transform.position,tgt),
											    10f*Time.deltaTime
				         					   );
    	}
    void StrafeAroundHindrance(Vector2 tgt)
    	{

    		transform.rotation=Quaternion.Slerp(transform.rotation,
											    Quaternion.Euler(0,0,90)*getRotationAngle(transform.position,tgt),
											    10f*Time.deltaTime
				         					   );
    	}
    bool detectObjectWithTag(Vector2 origin,Vector2 dir,string[] t_gs)
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


    bool DetectFenceInDirection(Vector2 dir)
		{
			Debug.DrawRay(transform.position-Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
				          dir.normalized*raycastLengthFront,
				          Color.red);
			Debug.DrawRay(transform.position,
				          dir.normalized*raycastLengthFront,
				          Color.red);
			Debug.DrawRay(transform.position+Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
				          dir.normalized*raycastLengthFront,
				          Color.red);
			
			bool hit1=detectObjectWithTag(transform.position-Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
							          dir.normalized*raycastLengthFront,
							          tags);			
			bool hit2=detectObjectWithTag(transform.position,
				          dir.normalized*raycastLengthFront,
				          tags);			
			bool hit3=detectObjectWithTag(transform.position+Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
				          dir.normalized*raycastLengthFront,
				          tags);
			return hit1 || hit2 || hit3;
		}
	bool DetectPlayerInDirection(Vector2 dir)
		{
			// Debug.DrawRay(transform.position-Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
			// 	          dir.normalized*raycastLengthFront,
			// 	          Color.red);
			// Debug.DrawRay(transform.position,
			// 	          dir.normalized*raycastLengthFront,
			// 	          Color.red);
			// Debug.DrawRay(transform.position+Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
			// 	          dir.normalized*raycastLengthFront,
			// 	          Color.red);
			
			bool hit1=detectObjectWithTag(transform.position-Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
							          dir.normalized*raycastLengthFront,
							          tags_player);			
			bool hit2=detectObjectWithTag(transform.position,
				          dir.normalized*raycastLengthFront,
				          tags_player);			
			bool hit3=detectObjectWithTag(transform.position+Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
				          dir.normalized*raycastLengthFront,
				          tags_player);
			return hit1 || hit2 || hit3;
		}
   	bool DetectFenceFront()
		{
			// Debug.DrawRay(transform.position-Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*sidewayOffset,
			// 	          transform.rotation*Vector2.right*raycastLengthFront,
			// 	          Color.red);
			// Debug.DrawRay(transform.position,
			// 	          transform.rotation*Vector2.right*raycastLengthFront,
			// 	          Color.red);
			// Debug.DrawRay(transform.position+Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*sidewayOffset,
			// 	          transform.rotation*Vector2.right*raycastLengthFront,
			// 	          Color.red);
			
			bool hit1=detectObjectWithTag(transform.position-Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*sidewayOffset,
							          transform.rotation*Vector2.right*raycastLengthFront,
							          tags);			
			bool hit2=detectObjectWithTag(transform.position,
				          transform.rotation*Vector2.right*raycastLengthFront,
				          tags);			
			bool hit3=detectObjectWithTag(transform.position+Quaternion.Euler(0,0,270)* transform.rotation*Vector2.right*sidewayOffset,
				          transform.rotation*Vector2.right*raycastLengthFront,
				          tags);
			return hit1 || hit2 || hit3;
		}
	bool DetectFenceLeft()
		{
			// Debug.DrawRay(transform.position-transform.rotation*Vector2.right*frontBackOffset,
			// 	          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthSideways,
			// 	          Color.red);
			// Debug.DrawRay(transform.position,
			// 	          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthSideways,
			// 	          Color.red);
			// Debug.DrawRay(transform.position+transform.rotation*Vector2.right*frontBackOffset,
			// 	          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthSideways,
			// 	          Color.red);

			bool hit1=detectObjectWithTag(transform.position-transform.rotation*Vector2.right*frontBackOffset,
				          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthSideways,
				          tags);			
			bool hit2=detectObjectWithTag(transform.position,
				          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthSideways,
				          tags);			
			bool hit3=detectObjectWithTag(transform.position+transform.rotation*Vector2.right*frontBackOffset,
				          Quaternion.Euler(0,0,90)*transform.rotation*Vector2.right*raycastLengthSideways,
				          tags);
			return hit1 || hit2 || hit3;
		}
	bool DetectFenceRight()
		{
			// Debug.DrawRay(transform.position-transform.rotation*Vector2.right*frontBackOffset,
			// 	          Quaternion.Euler(0,0,270)*transform.rotation*Vector2.right*raycastLengthSideways,
			// 	          Color.red);
			// Debug.DrawRay(transform.position,
			// 	          Quaternion.Euler(0,0,270)*transform.rotation*Vector2.right*raycastLengthSideways,
			// 	          Color.red);
			// Debug.DrawRay(transform.position+transform.rotation*Vector2.right*frontBackOffset,
			// 	          Quaternion.Euler(0,0,270)*transform.rotation*Vector2.right*raycastLengthSideways,
			// 	          Color.red);


			bool hit1=detectObjectWithTag(transform.position-transform.rotation*Vector2.right*frontBackOffset,
				          Quaternion.Euler(0,0,270)*transform.rotation*Vector2.right*raycastLengthSideways,
				          tags);			
			bool hit2=detectObjectWithTag(transform.position,
				          Quaternion.Euler(0,0,270)*transform.rotation*Vector2.right*raycastLengthSideways,
				          tags);			
			bool hit3=detectObjectWithTag(transform.position+transform.rotation*Vector2.right*frontBackOffset,
				          Quaternion.Euler(0,0,270)*transform.rotation*Vector2.right*raycastLengthSideways,
				          tags);
			return hit1 || hit2 || hit3;
		}
}
