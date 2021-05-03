using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimpleControlledMovement : MonoBehaviour {

	

	CharacterController characterController;
	Collider myCollider;
	public float turnSpeed=10f;
	public float moveSpeed=1f;
	Camera mainCamera;	
	Animator anim;
	public float angleStep=5f;
	PlayerAttackAI p_attack_ai;
	public string enemyString;
	public bool isAI;
	public float scanRadius=10f;
	GameObject target;
	public float aiTurnSpeed=5f,aiMoveSpeed=10f,aiProximDistance=10f;

	float lastAttackClocked=-90f;
	public float intervalBetweenAttacks=3f;
	public GameObject explosionx;

	public bool defensive;
	float dclock=0f;
	
	void Start () 
		{
		characterController = GetComponent<CharacterController>();
		myCollider          = GetComponent<Collider>();
		mainCamera=Camera.main;
		anim= GetComponent<Animator>();
		p_attack_ai=GetComponent<PlayerAttackAI>();
		}

	void ScanArea()
		{
			Collider[] hitColliders= Physics.OverlapSphere(transform.position,scanRadius);
			foreach(Collider col in hitColliders)
				{
					if(col.gameObject.tag==enemyString)
						{
							target=col.gameObject;
							/*
							Debug.Log(gameObject.tag+
								" says"+ enemyString +"found, seperation"+
								Vector3.Distance(target.transform.position,transform.position) 
								);*/
						}
				}
		}

  void OnDrawGizmosSelected()
    	{
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1	);
    	}
	
	
	void Update () 
		{
			if(!isAI)
				{
				ListenForInput();
			 	Vector3 moveDirection=Vector3.zero;
				if(Input.GetAxis("Vertical")>0)
				 	{
					 moveDirection= Input.GetAxis("Vertical")*transform.forward;
					 anim.SetFloat("Walk",1.0f);
					}
				else 
					{
					anim.SetFloat("Walk",0.0f);	
					}
			 	moveDirection=moveDirection.normalized;
			 	transform.rotation=Quaternion.Slerp(transform.rotation,
			 									 Quaternion.Euler(0f,transform.eulerAngles.y+angleStep * Input.GetAxis("Horizontal"),0f),
			 									 turnSpeed* Time.deltaTime);
			 	characterController.Move(moveDirection * moveSpeed);
		 		}
		 	else
		 		{
		 			if(!target)
		 				ScanArea();
		 			MoveTowardsTarget();

		 		}


		}
	void MoveTowardsTarget()
		{
			if(!GetComponent<PlayerHealthAI>().alive)
				{
				if(dclock==0f)
					{
					dclock=Time.time;
					p_attack_ai.PlayDeathAnimation();
					Destroy(gameObject,7.5f);					
					}
				else if(Time.time-dclock > 7.4f && dclock!=15f)
					{
						Instantiate(explosionx,transform.position,Quaternion.identity);
						dclock=15f;
					}
				return;
				}
			if(!target || !target.GetComponent<PlayerHealthAI>().alive)
				return;
			
			Vector3 targetPos=new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z);

			if(Vector3.Distance(transform.position,targetPos)> aiProximDistance)
				{
				transform.rotation=Quaternion.Slerp(transform.rotation,
												Quaternion.LookRotation(targetPos-transform.position),
												aiTurnSpeed * Time.deltaTime
												);
				characterController.Move(transform.forward * aiMoveSpeed);
				anim.SetFloat("Walk",1.0f);
				}
			else
				{
				anim.SetFloat("Walk",0.0f);
				Attack_AI();
				}
			

		}

	void Attack_AI()
		{			
			if(Time.time-lastAttackClocked > intervalBetweenAttacks)
				{
				lastAttackClocked=Time.time;
				if(!defensive )
				p_attack_ai.LaunchRandomMinorAttack();
				else if(!p_attack_ai.CanShieldUp())
				p_attack_ai.LaunchRandomMinorAttack();	 

				
				}
		}


	void ListenForInput()
		{
			if(Input.GetMouseButtonDown(0))
				{
					p_attack_ai.LaunchRandomMajorAttack();
				}
			else if(Input.GetMouseButtonDown(1))
					p_attack_ai.LaunchRandomMinorAttack();
		}
}
