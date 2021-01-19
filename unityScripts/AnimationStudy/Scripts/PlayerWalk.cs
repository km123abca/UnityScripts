using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour {

	public float rotationSpeed=3f;
	private float rotateY;
	PlayerAnimation playerAnim;
	float h,v;
	public Transform groundCheckPosition;
	public float jumpPower=200f;
	public float radius = 0.3f;
	public LayerMask groundLayer;
	Rigidbody myBody;
	bool isGrounded,hasJumped;
	void Awake () 
		{
		 myBody=GetComponent<Rigidbody>();
		 playerAnim=GetComponent<PlayerAnimation>();
		
		}
	

	void Update () 
		{
			CheckMovement();
			CheckForAttack();
			CheckGroundCollisionAndJump();
			AnimatePlayer();
		}
	void CheckGroundCollisionAndJump()
		{
			isGrounded=Physics.OverlapSphere(groundCheckPosition.position,
											 radius,
											 groundLayer
				                            ).Length>0;
			if(Input.GetKeyDown(KeyCode.Space))
				{
					if(isGrounded)
						{
							myBody.AddForce(new Vector3(0,jumpPower,0));
							hasJumped=true;
							playerAnim.Jumped();
						}
				}
		}
	void CheckForAttack()
		{
			if(Input.GetMouseButtonDown(0))
				{
					playerAnim.Attack1();
				}
			else if(Input.GetMouseButtonDown(1))
				{
					playerAnim.Attack2();	
				}
		}
	void CheckMovement()
		{
			h=Input.GetAxis("Horizontal");
			v=Input.GetAxis("Vertical");
			rotateY-=h*rotationSpeed;
			transform.localRotation = Quaternion.AngleAxis(rotateY,Vector3.up);
		}
	void AnimatePlayer()
		{
			if(v!=0)
				{
					playerAnim.PlayerWalks(true);
				}
			else
				{
					playerAnim.PlayerWalks(false);
				}
		}
	void OnCollisionEnter(Collision target)
		{
			if(target.gameObject.tag=="Ground")
				{
					if(hasJumped)
					{
					hasJumped=false;
					playerAnim.EndJump();
					}
				}
		}
}
