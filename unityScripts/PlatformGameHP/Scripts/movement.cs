using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

	Animator anim;
	public float speed=4f;
	Rigidbody2D rb;

	public float hoffset=2f;
	public float rcGdDist=4f;
	public float jumpVelocity=2f;
	string[] gBlocks=new string[]{"tile"};
	int temp=0;
	bool isJumping;
	float jumpingClock=0;
	float maxJumpingClock=2f;
	void DebugDisp(string strr)
		{
		  temp+=1;
		  Debug.Log(temp+". "+strr);
		}
	void Start () {
		anim=GetComponent<Animator>();
		rb=GetComponent<Rigidbody2D>();
	}
	
	
	void Update () {
		float horiz=Input.GetAxis("Horizontal");
		rb.velocity=Vector2.right * horiz * speed;
		anim.SetBool("running",Mathf.Abs(horiz) > 0.1f);
		FlipPlayer(horiz);
		bool jumpButton=Input.GetButton("Jump");
		if(!isJumping && jumpButton && IsOnGround() ) 
			{				
				isJumping=true;
				jumpingClock=Time.time;
			}
		
		if(isJumping && Time.time-jumpingClock < maxJumpingClock)
			{
				rb.velocity=new Vector2(rb.velocity.x,jumpVelocity);
			}
		if(!jumpButton && isJumping)
			{
				isJumping=false;
			}
		
	}

	void FlipPlayer(float h)
	{
		Vector3 scale=transform.localScale;
		if(h > 0)
			scale.x=Mathf.Abs(scale.x);
		else if(h < 0)
			scale.x=-1 * Mathf.Abs(scale.x);
		transform.localScale=scale;
	}
	bool IsOnGround()
		{
			Vector2 playerPos=new Vector2(transform.position.x,transform.position.y);
			Vector2 pos1=playerPos-Vector2.right*hoffset;
			Vector2 pos2=playerPos;
			Vector2 pos3=playerPos+Vector2.right*hoffset;
			Vector2 down=Quaternion.Euler(0,0,-90)*Vector2.right*rcGdDist;
			Debug.DrawRay(pos1,down,Color.red);
			Debug.DrawRay(pos2,down,Color.red);
			Debug.DrawRay(pos3,down,Color.red);
			bool hit1=detectObjectsWithTag(pos1,down,gBlocks);
			bool hit2=detectObjectsWithTag(pos2,down,gBlocks);
			bool hit3=detectObjectsWithTag(pos3,down,gBlocks);
			return hit1||hit2||hit3;
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
}
