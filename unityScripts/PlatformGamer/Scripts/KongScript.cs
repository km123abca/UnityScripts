using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongScript : MonoBehaviour 
	{

	Rigidbody2D rb;
	public float speed=5f;
	public int dir=1;
	string nextPoint="stopPoint2";
	public int Dir
    {
        get {return dir;}
        // set {x=value;}
    }
	void Start () 
		{
		  rb=GetComponent<Rigidbody2D>();
		}
	
	
	void Update () 
		{
			rb.velocity=dir * Vector2.right*speed;

		}

	void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.tag==nextPoint)
				{
					// Debug.Log("Collision with stop point");
					dir*=-1;
					nextPoint=nextPoint=="stopPoint2"?"stopPoint1":"stopPoint2";	
									
					FlipAccordingly(dir);
				}
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

	}
