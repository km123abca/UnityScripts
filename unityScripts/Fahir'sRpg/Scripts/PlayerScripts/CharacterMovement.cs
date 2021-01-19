using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	private MovementMotor motor;
	public float move_Magnitude =0.05f;
	public float speed =0.7f;
	public float speed_Move_WhileAttack=0.1f;
	public float speed_Attack=1.5f;
	public float turnSpeed=10f;
	public float speed_Jump=20f;
	float speed_Move_Multiplier=1f;
	Vector3 direction;
	Animator anim;
	Camera mainCamera;
	string PARAMETER_STATE="State";
	string PARAMETER_ATTACK_TYPE="AttackType";
	string PARAMETER_ATTACK_INDEX="AttackIndex";
	public AttackAnimation[] attack_Animations;
	public string[] combo_AttackList; 
	public int combo_Type;
	private int attack_Index=0;
	string[] combo_List;
	int attack_Stack;
	float attack_Stack_TimeTemp;



	bool isAttacking;
	GameObject atkPoint;
	void Awake()
		{
			motor=GetComponent<MovementMotor>();
			anim=GetComponent<Animator>();
		}
	void Start () 
		{
			anim.applyRootMotion=false; 
			mainCamera=Camera.main;
			atkPoint=GameObject.Find("Player Attack Point");
			atkPoint.SetActive(false);
		}
	
	void Update () 
		{

		  HandleAttackAnimations();
		  if(MouseLock.MouseLocked)
		  	{
		  		if(Input.GetButtonDown("Fire1"))
		  			{
		  				Attack();
		  			}
		  		if(Input.GetButtonDown("Fire2"))
		  			{
		  				Attack();	
		  			}
		  	}
		  MovementAndJumping();

		}


	void AnimationMove(float magnitude)
		{
			if(magnitude > move_Magnitude)
				{
					float speed_Animation= magnitude * 2f;
					if(speed_Animation < 1f)
						{
							speed_Animation=1f;
						}
					if(anim.GetInteger(PARAMETER_STATE)!=2)
						{
							anim.SetInteger(PARAMETER_STATE,1);
							anim.speed= speed_Animation;
						}
				}
			else
				{
				if(anim.GetInteger(PARAMETER_STATE)!=2)
					{
						anim.SetInteger(PARAMETER_STATE,0);
					}
				}
		}

	private Vector3 MoveDirection
		{
			get{ return direction;}
			set { 
					direction= value * speed_Move_Multiplier;
					if(direction.magnitude > 0.1f)
						{
							var newRotation= Quaternion.LookRotation(direction);
							transform.rotation=Quaternion.Slerp(transform.rotation,newRotation,Time.deltaTime*turnSpeed) ;
						}
					direction *=speed * (Vector3.Dot(transform.forward,direction)+1f) * 5f;
					motor.Move(direction);
					AnimationMove(motor.charController.velocity.magnitude * 0.1f);
				}
		}
	void Moving(Vector3 dir,float mult)
		{
			// speed_Move_Multiplier = 1 * mult;
			// MoveDirection=dir; 
			if(isAttacking)
				{
					speed_Move_Multiplier = speed_Move_WhileAttack * mult;
				}
			else
				speed_Move_Multiplier = 1 * mult;
			MoveDirection=dir;
		}
	void Jump()
		{
			motor.Jump(speed_Jump);
		}
	void MovementAndJumping()
		{
			Vector3 moveInput=Vector3.zero;
			Vector3 forward= Quaternion.AngleAxis(-90,Vector3.up) * mainCamera.transform.right;
			moveInput += forward * Input.GetAxis("Vertical");
			moveInput += mainCamera.transform.right * Input.GetAxis("Horizontal");
			moveInput.Normalize();
			Moving(moveInput.normalized,1f);

			if(Input.GetKey(KeyCode.Space))
				{
					Jump();
				}
		}
	void FightAnimation()
		{
			if(combo_List != null && attack_Index >= combo_List.Length)
				{
					ResetCombo();
				}
			if(combo_List!=null && combo_List.Length > 0)
				{
					int motionIndex = int.Parse(combo_List[attack_Index]);
					if (motionIndex < attack_Animations.Length)
						{
							anim.SetInteger(PARAMETER_STATE,2);
							anim.SetInteger(PARAMETER_ATTACK_TYPE,combo_Type); 
							anim.SetInteger(PARAMETER_ATTACK_INDEX,attack_Index);
						}
				}
		}
	void ResetCombo()
		{
			attack_Index=0;
			// attack_Stack=0; //kitchu commented
			isAttacking=false;
		}
	void HandleAttackAnimations()
		{
			
			if(Time.time > attack_Stack_TimeTemp + 0.5f)//kitchu changed 0.5 to 5
				attack_Stack=0;
				 //kitchu commented the entire block
			combo_List=combo_AttackList[combo_Type].Split("," [0]);
			if(anim.GetInteger(PARAMETER_STATE )==2)
				{
					anim.speed= speed_Attack; 
					AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
					if(stateInfo.IsTag("Attack"))
						{
							int motionIndex= int.Parse(combo_List[attack_Index]);
							if(stateInfo.normalizedTime > 0.9f)
								{
									anim.SetInteger(PARAMETER_STATE,0); 	
									isAttacking=false;
									attack_Index++;
									if(attack_Stack > 1)
										{
											//attack_Stack=(attack_Stack-1<0)?0:attack_Stack-1;//kitchu added
											FightAnimation();
										}
									else
										{
										//attack_Stack=0;//kitchu added
										if(attack_Index >= combo_List.Length)
											{
												ResetCombo();
											}
										}
								}	
						}
				}
		}

	void Attack()
		{
			if(attack_Stack < 1 || (Time.time > attack_Stack_TimeTemp +0.2f  && Time.time < attack_Stack_TimeTemp + 1f))
				{
					attack_Stack++;					
					attack_Stack_TimeTemp=Time.time;
				}
			FightAnimation();
		}

	void Attack_Began()
		{
			atkPoint.SetActive(true);
		}
	void Attack_End()
		{
			atkPoint.SetActive(false);
		}
}
