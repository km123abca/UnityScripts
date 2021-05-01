using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLife : MonoBehaviour 
	{
	public GameObject lchild;
	float spawnClock;
	public float spawnInterval=1f,yseperation=0.6f;
	public int numChildren;
	int spawnedChildren;
	public float lifeTime=4f;
	void Start () 
		{
		
		}
	
	
	void Update () 
		{
			for(int i=0;i < numChildren;i++)
				{
				  if(Time.time > i*spawnInterval && spawnedChildren < i+1 )
				  			{
				  				GameObject g=Instantiate(lchild,transform.position,Quaternion.identity);
				  				g.transform.SetParent(transform);
				  				g.transform.localPosition=Vector2.zero+new Vector2(0f,yseperation*i);
				  				g.transform.localRotation=Quaternion.Euler(0,0,90);
				  				spawnedChildren+=1;

				  			}
				  					
				}
			if(Time.time > lifeTime)
				{
					Destroy(gameObject);
				}
			
		}

	}
