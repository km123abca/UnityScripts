using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MannyScript : MonoBehaviour 
	{

	Rigidbody2D rb;
	public float speed=5f;
	public float jumpSpeed=20f;
	public GameObject Torso;
	MannyAnimationControl tscript;
	public float halfht,raycastDistanceDown,raycastRight;
	public string[] tagsToSearch;
	bool jumping;
	// public int maxJumps=2;
	// int jumps=0;
	public float maxJumpTime=4f;
	float jumpTimer=0f;
	GameManager gameManagerScript;
	public float maxHealth=100f;
	float health;
	Vector2 addlVelocity=Vector2.zero;



	public void DoStuffOnOutOfBounds()
		{
			// Debug.Log("Im invisible");
			GameObject.Find("FinalText").GetComponent<Text>().text="YOU DIED";
			gameManagerScript.GameOver=true;
			Destroy(gameObject);
		}
	void Start () 
		{
		  health=maxHealth;
		  gameManagerScript=GameObject.Find("GameManager").GetComponent<GameManager>();
		  rb=GetComponent<Rigidbody2D>();
		  tscript=Torso.GetComponent<MannyAnimationControl>();
		}
	public void TakeDamage(float dmg)
		{
			health-=dmg;
			GameObject.Find("GreenBar").GetComponent<Image>().fillAmount=health/maxHealth;
			if(health <= 0)
				{
					DoStuffOnOutOfBounds();
				}
		}
	public void Revive(float dmg)
		{

			health+=dmg;
			if(health > maxHealth) health-=dmg;
			GameObject.Find("GreenBar").GetComponent<Image>().fillAmount=health/maxHealth;
			
		}	
	void Update () 
		{
			// DetectGround();
			// WallOnRight(0.5f);
			if(transform.position.y < -10)
				TakeDamage(0.5f);
				// DoStuffOnOutOfBounds();
			float horizontalInput= Input.GetAxis("Horizontal");
			float vertMove= Input.GetAxis("Jump");
			PointManny(horizontalInput);

			// float verticalInput  = Input.GetAxis("Vertical");

			float horizontalVelocity= speed * horizontalInput;
			float verticalVelocity=0f;

			if(DetectGround() && vertMove > 0 && !jumping)
				{
					jumping=true;
					tscript.AnimateJump();

				}
			// else if(DetectGround() && jumping)
			// 	{
			// 		jumping= false;
			// 		jumpTimer=0f;
			// 	}

			if(jumping  && vertMove > 0 && jumpTimer < maxJumpTime)
				{
					verticalVelocity= jumpSpeed;
					jumpTimer+= Time.deltaTime;
				}
			else if(jumping && (vertMove==0 || jumpTimer >= maxJumpTime))
				{
					jumping=false;
					jumpTimer=0f;
					verticalVelocity=0f;
				}
			else
				{
					verticalVelocity=0f;
				}

			if(!DetectGround() && vertMove > 0 && Mathf.Abs(horizontalInput)>0 && WallOnRight(horizontalInput))	
				{
					Debug.Log("Detecting wall jump");
					verticalVelocity=1.5f * jumpSpeed;	
				}

			if(!transform.parent)
			rb.velocity=new Vector2(horizontalVelocity,verticalVelocity);
			else
			rb.velocity=new Vector2(horizontalVelocity,verticalVelocity)+transform.parent.GetComponent<PlatformMover>().GetVel();

			
			tscript.Move(horizontalVelocity);		
		}
	void PointManny(float x)
		{			
			Vector3 scale= transform.localScale;
			if(x > 0)
				scale.x =Mathf.Abs(scale.x);
			else if(x < 0)
				scale.x=-1* Mathf.Abs(scale.x);
			transform.localScale= scale;
			
		}
	bool DetectGround()
		{
			Vector2 present2DPos=new Vector2(transform.position.x,transform.position.y);
			RaycastHit2D[] hits=Physics2D.RaycastAll(present2DPos+ halfht * Vector2.down,Vector2.down,raycastDistanceDown);

			Debug.DrawRay(present2DPos+ halfht * Vector2.down,raycastDistanceDown * Vector2.down,Color.red);
			foreach(RaycastHit2D hit in hits)
				{
					foreach(string x in tagsToSearch)
						{
						if(hit.collider.tag==x)
							{
								// Debug.Log("true");
								return true;
							}
						}
					
				}
				// Debug.Log("false");
			return false;
		}

	bool WallOnRight(float hori)
		{
			int k=1;
			if(hori < 0)
				k=-1;
			Vector2 present2DPos=new Vector2(transform.position.x,transform.position.y);
			RaycastHit2D[] hits1=Physics2D.RaycastAll(present2DPos,k * Vector2.right,raycastRight);
			RaycastHit2D[] hits2=Physics2D.RaycastAll(present2DPos + halfht * Vector2.down,k * Vector2.right,raycastRight);
			RaycastHit2D[] hits3=Physics2D.RaycastAll(present2DPos - halfht * Vector2.down,k * Vector2.right,raycastRight);

			//#######################################
			RaycastHit2D[] hits=new RaycastHit2D[hits1.Length + hits2.Length + hits3.Length];
			hits1.CopyTo(hits,0);
			hits2.CopyTo(hits,hits1.Length);
			hits3.CopyTo(hits,hits1.Length + hits2.Length);
			//#######################################

			Debug.DrawRay(present2DPos,k * raycastRight * Vector2.right,Color.red);
			Debug.DrawRay(present2DPos + halfht * Vector2.down,k * raycastRight * Vector2.right,Color.red);
			Debug.DrawRay(present2DPos - halfht * Vector2.down,k * raycastRight * Vector2.right,Color.red);
			foreach(RaycastHit2D hit in hits)
				{
					foreach(string x in tagsToSearch)
						{
						if(hit.collider.tag==x)
							{
								// Debug.Log("true");
								return true;
							}
						}
					
				}
				// Debug.Log("false");
			return false;	
		}
	}
