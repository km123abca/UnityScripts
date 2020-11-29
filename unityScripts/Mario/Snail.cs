using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : MonoBehaviour
{
	public float speed=2f;
	Vector2 direction=Vector2.right;

    void FixedUpdate()
    	{
    		GetComponent<Rigidbody2D>().velocity=direction* speed;
    	}
    void OnTriggerEnter2D(Collider2D col)
    	{
    		transform.localScale=new Vector2(-1*transform.localScale.x,transform.localScale.y);
    		direction=new Vector2(-1* direction.x,direction.y);
    	}
    void OnCollisionEnter2D(Collision2D col)
    	{

    		if(col.gameObject.name=="Manny")
    			{
    				Debug.Log("Manny at:"+col.contacts[0].point.y);
    				Debug.Log("Snail at:"+transform.position.y);
    				float ht=col.gameObject.GetComponent<Manny>().height;
    				if(col.contacts[0].point.y+.009769f > transform.position.y)
    					{

    						GetComponent<Animator>().SetTrigger("Dead");
    						GetComponent<Collider2D>().enabled=false;
    						direction=new Vector2(direction.x,-1);
    						DestroyObject(gameObject,3);
    					}
    				else
    					{

    						SoundManager.Instance.PlayOneShot(SoundManager.Instance.mannyDies);
    						DestroyObject(col.gameObject,0.5f);
    					}
    			}
    	}
}
