using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swordsman : MonoBehaviour 
	{

	Rigidbody2D rb;
	int dir=1;
	public float speed=5f;
	public float raycastLengthFront=10f,raycastLengthBack=5f;
	public float proximDistance=1f;
	public float sidewayOffset=1f;
	bool chasing,returningFromEdge;
	bool attacking;
	public LayerMask PlayerLayer;

	float health;
	public float maxHealth=100f;
	public GameObject redBar,greenBar,cpoof;

	public void InflictDamage()
		{
			// Debug.Log("ok");
			if(DetectPlayerInDirection(dir * Vector2.right,proximDistance,"Player"))
				GameObject.FindGameObjectWithTag("Player").GetComponent<MannyScript>().TakeDamage(10f);
		}
	void Start () 
		{
		  rb=GetComponent<Rigidbody2D>();
		  health=maxHealth;
		 
		}
	
	public void TakeDamage(float dmg)
		{
			// Debug.Log("damage received");
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
	void Update () 
		{
			if(Physics2D.OverlapCircle(transform.position,raycastLengthBack/2,PlayerLayer))
				{
					attacking=true;
				}
			else
					attacking=false;

			if(!attacking)
				{
				rb.velocity= dir * speed * Vector2.right;
				
				if(DetectPlayerInDirection(dir * Vector2.right,raycastLengthFront,"Player"))
					{
						chasing=true;
					}
				else if(DetectPlayerInDirection(-dir * Vector2.right,raycastLengthBack,"Player"))
					{
						chasing=true;
						dir *=-1;
						FlipAccordingly(dir);
					}
				else
					    chasing	=false;
				}
			else
				{
				 rb.velocity=Vector2.zero;
				 transform.Find("sman_idle").GetComponent<SwordsmanAnimation>().StartAttack();
				}

			transform.Find("sman_idle").GetComponent<SwordsmanAnimation>().Move(rb.velocity.magnitude);

		}
    void FlipAccordingly(int dir)
    	{
    		Vector2 sc=transform.localScale;
    		if(dir > 0)
    			{
    				sc.x= Mathf.Abs(sc.x);
    			}
    		else
    				sc.x= -Mathf.Abs(sc.x);
    		transform.localScale=sc;
    	}
	void OnTriggerEnter2D(Collider2D col)	
		{
			if (!chasing  && col.gameObject.tag=="stopPoint" )  
				{

				if(!returningFromEdge)
					{
					dir *=-1;
					FlipAccordingly(dir);	
					}
				else
					{
					returningFromEdge=false;
					}
					
				}
			else if(col.gameObject.tag=="edgePoint")
				{
					dir *=-1;
					FlipAccordingly(dir);
					returningFromEdge=true;
				}
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

    bool DetectPlayerInDirection(Vector2 dir,float raycastLengthFront,string tag)
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
							          	  tag);			
			bool hit2=detectObjectWithTag(transform.position,
				          				  dir.normalized*raycastLengthFront,
				          				  tag);			
			bool hit3=detectObjectWithTag(transform.position+Quaternion.Euler(0,0,270)* dir.normalized*sidewayOffset,
				          				  dir.normalized*raycastLengthFront,
				          				  tag);
			return hit1 || hit2 || hit3;
		}
	}
