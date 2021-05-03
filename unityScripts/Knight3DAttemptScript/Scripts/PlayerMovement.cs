using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
	{

	CharacterController characterController;
	Collider myCollider;
	public float turnSpeed=10f;
	Camera mainCamera;
	public float moveSpeed=10f;
	Animator anim;
	public float distanceToGround=0.2f;
	public float initialVerticalVelocity=20f,g_small=5f;
	float verticalVelocity=0f;



	void Start () 
		{
			characterController = GetComponent<CharacterController>();
			myCollider          = GetComponent<Collider>();
			mainCamera=Camera.main;
			anim= GetComponent<Animator>();
		}
	
	
	void Update () 
		{
			Vector3 forward_direction = Quaternion.AngleAxis(-90,Vector3.up) * mainCamera.transform.right;
			Vector3 right_direction   = mainCamera.transform.right;
			Vector3 targetDirection= Input.GetAxis("Vertical") * forward_direction+Input.GetAxis("Horizontal") * right_direction;

			//jumping functionality
			/*
			if(Input.GetKeyDown(KeyCode.Space))
				{
					verticalVelocity=initialVerticalVelocity;
				}
			
			verticalVelocity-=g_small;
			if(verticalVelocity!=0f)
				characterController.Move(Vector3.up * verticalVelocity);
				*/
			//jumping functionality ends
							
			
			targetDirection= targetDirection.normalized;

			if(targetDirection.magnitude > 0.1f)
				{
				TurnTowardsDirection(targetDirection);
				// targetDirection.y+=verticalVelocity;
				characterController.Move(targetDirection * moveSpeed);

				anim.SetFloat("Walk",1.0f);
				}
			else
				{
					anim.SetFloat("Walk",0.0f);
				}
			if(OnGroundCheck())
				verticalVelocity=0f;
			Debug.DrawRay(myCollider.bounds.center,-Vector3.up*distanceToGround,Color.red);

		}

	void TurnTowardsDirection(Vector3 dir)
		{
			var newRotation = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Slerp(transform.rotation,newRotation,Time.deltaTime * turnSpeed);
		}
	public bool OnGroundCheck()
		{
			RaycastHit hit;
			if(characterController.isGrounded)
				{
					return true;
				}
			if(Physics.Raycast(myCollider.bounds.center,-Vector3.up,out hit,distanceToGround))
				{
					return true;
				}
			return false;
		}
	}
