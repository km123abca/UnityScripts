using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour 
	{

	public Image fillWaitImage_1;
	public Image fillWaitImage_2;
	public Image fillWaitImage_3;
	public Image fillWaitImage_4;
	public Image fillWaitImage_5;
	public Image fillWaitImage_6;

	int[] fadeImages = new int[]{0,0,0,0,0,0};
	Animator anim;
	bool canAttack=true;
	public bool isAttacking;

	CharacterMovement charMoveScript;
	void Awake () 
		{
		  anim=GetComponent<Animator>();
		  charMoveScript=GetComponent<CharacterMovement>();
		}
	
	
	void Update () 
		{
			if(!(!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Stand") ))
				isAttacking=true;
			else
				isAttacking=false;
			if(!anim.IsInTransition(0))
				canAttack=true;
			else
				canAttack=false;
			CheckInput();
			CheckToFade();
		}


	bool CheckForRunningAttack()
		{
			if(charMoveScript.RunningForAWhileTimer > charMoveScript.maxRunningForAWhile)
				return true;
			return false;

		}
	public bool IsAttacking_info
		{
			get {return isAttacking;}
		}
	void CheckInput()
		{

			if(Input.GetMouseButtonDown(0))
				{					
				if(fadeImages[0]!=1 && canAttack)
						{
						
						fadeImages[0]=1;
						anim.SetInteger("Atk",1);
						}
				}
			// else if(CheckForRunningAttack() && Input.GetKeyDown(KeyCode.Alpha2))
			else if(Input.GetMouseButtonDown(1))
				{
					if(fadeImages[1]!=1 && canAttack)
						{
						
						fadeImages[1]=1;
						anim.SetInteger("Atk",2);
						}
						
				}
			else if(Input.GetKeyDown(KeyCode.Alpha1))
				{
					if(fadeImages[2]!=1 && canAttack)
						{
						
						fadeImages[2]=1;
						anim.SetInteger("Atk",3);
						}
				}
			else if(Input.GetKeyDown(KeyCode.Alpha2))
				{
					if(fadeImages[3]!=1 && canAttack)
						{
						
						fadeImages[3]=1;
						anim.SetInteger("Atk",4);
						}
				}
			else if(Input.GetKeyDown(KeyCode.Alpha3))
				{
					if(fadeImages[4]!=1 && canAttack)
						{
						
						fadeImages[4]=1;
						anim.SetInteger("Atk",5);
						}
				}
			else if(Input.GetKeyDown(KeyCode.Alpha4))
				{
					if(fadeImages[5]!=1 && canAttack)
						{
						
						fadeImages[5]=1;
						anim.SetInteger("Atk",6);
						}
				}
			else
				{	
									
					anim.SetInteger("Atk",0);

				}
		}
	bool FadeAndWait(Image fadeImg,float fadeTime)
	{
		bool faded = false;
		if(fadeImg==null)
			return faded;
		if(!fadeImg.gameObject.activeInHierarchy)
			{
				fadeImg.gameObject.SetActive(true);
				fadeImg.fillAmount=1f;
			}
		fadeImg.fillAmount-=fadeTime*Time.deltaTime;
		if(fadeImg.fillAmount <= 0.0f)
			{
				fadeImg.gameObject.SetActive(false);
				faded=true;
			}
		return faded;
	}
	void CheckToFade()
		{
			if(fadeImages[0]==1)
				{
					if(FadeAndWait(fillWaitImage_1,1.0f))
						{
							fadeImages[0]=0;
						}
				}
			if(fadeImages[1]==1)
				{
					if(FadeAndWait(fillWaitImage_2,0.7f))
						{
							fadeImages[1]=0;
						}
				}
			if(fadeImages[2]==1)
				{
					if(FadeAndWait(fillWaitImage_3,0.1f))
						{
							fadeImages[2]=0;
						}
				}
			if(fadeImages[3]==1)
				{
					if(FadeAndWait(fillWaitImage_4,0.2f))
						{
							fadeImages[3]=0;
						}
				}
			if(fadeImages[4]==1)
				{
					if(FadeAndWait(fillWaitImage_5,0.3f))
						{
							fadeImages[4]=0;
						}
				}
			if(fadeImages[5]==1)
				{
					if(FadeAndWait(fillWaitImage_6,0.08f))
						{
							fadeImages[5]=0;
						}
				}

		}

	}
