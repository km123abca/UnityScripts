using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackConsumers : MonoBehaviour {
    public GameObject groundImpactPrefab,
    				  fireTornadoPrefab,
    				  fireShieldPrefab,
    				  kickFxPrefab;
    				  
    public GameObject groundImpactSpawnPoint,
    				  FireTornadoPrefabSpawnPoint,
    				  fireShieldSpawnPoint,
    				  kickFxSpawnPoint;

    public GameObject healPrefab,thunderPrefab;
	public LayerMask el;
	void Start () 
		{
		
		}
	
	
	void Update () 
		{
		
		}

	void GroundImpact()
		{
			// Debug.Log("After Effect of the first Attack");

			GameObject fxo=	Instantiate(groundImpactPrefab,groundImpactSpawnPoint.transform.position,Quaternion.identity);
			fxo.SendMessage("GetEnemyLayer",el);
		}
	void Kick()
		{
			GameObject fxo=Instantiate(kickFxPrefab,kickFxSpawnPoint.transform.position,Quaternion.identity);
			fxo.SendMessage("GetEnemyLayer",el);
		}
	void FireTornado()
		{
			GameObject fxo=Instantiate(fireTornadoPrefab,fireShieldSpawnPoint.transform.position,Quaternion.identity);
			fxo.SendMessage("GetEnemyLayer",el);	
		}
	void FireShield()
		{
			GameObject fireObj=Instantiate(fireShieldPrefab,
										   fireShieldSpawnPoint.transform.position,
										   Quaternion.identity) as GameObject;
			fireObj.SendMessage("GetParent",gameObject);
			fireObj.transform.SetParent(transform);
		}
	void Heal()
		{
			Vector3 temp = transform.position;
			temp.y+=2f;
			GameObject healObj=Instantiate(healPrefab,temp,Quaternion.identity) as GameObject;
			healObj.transform.SetParent(transform);
		}

	//d
	void ThunderAttack()
		{
			for(int i=0;i<8;i++)
			{
				Vector3 pos=Vector3.zero;
				if(i==0)
					{
						pos=new Vector3(transform.position.x-4f,
										transform.position.y+2f,
										transform.position.z);
					}
				else if(i==1)
					{
						pos=new Vector3(transform.position.x+4f,
										transform.position.y+2f,
										transform.position.z);
					}
				else if(i==2)
					{
						pos=new Vector3(transform.position.x,
										transform.position.y+2f,
										transform.position.z-4f);
					}
				else if(i==3)
					{
						pos=new Vector3(transform.position.x,
										transform.position.y+2f,
										transform.position.z+4f);
					}
				else if(i==4)
					{
						pos=new Vector3(transform.position.x+2.5f,
										transform.position.y+2f,
										transform.position.z+2.5f);
					}
				else if(i==5)
					{
						pos=new Vector3(transform.position.x-2.5f,
										transform.position.y+2f,
										transform.position.z+2.5f);
					}
				else if(i==6)
					{
						pos=new Vector3(transform.position.x-2.5f,
										transform.position.y+2f,
										transform.position.z-2.5f);
					}
				else if(i==7)
					{
						pos=new Vector3(transform.position.x+2.5f,
										transform.position.y+2f,
										transform.position.z+2.5f);
					}
				GameObject fxo=Instantiate(thunderPrefab,pos,Quaternion.identity); 
				fxo.SendMessage("GetEnemyLayer",el);
			}
			
		}
	//d
}
