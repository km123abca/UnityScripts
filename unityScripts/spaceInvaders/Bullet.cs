using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed=30;
    private Rigidbody2D rigidBody;
    public Sprite explodedalienImage;
    public GameObject smoke;
    void Start()
    {
        rigidBody=GetComponent<Rigidbody2D>();
        rigidBody.velocity=Vector2.up * speed;
        // Instantiate(smoke,transform.position,Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D col)
    	{
    		if(col.tag=="Wall")
    			{
    				Destroy(gameObject);
    			}
    		else if(col.tag=="Alien")
    			{
                    
                    col.GetComponent<SpriteRenderer>().sprite=explodedalienImage;
                    GameObject g=Instantiate(smoke,transform.position,Quaternion.identity) as GameObject;
                    
                    if(! col.GetComponent<Alien>().gotHit)
                        {
        				SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDies);
                        Destroy(gameObject);
                        cms.increaseScore(10);                     
                        col.GetComponent<Alien>().gotHit=true;
                        DestroyObject(col.gameObject,0.5f);
                        }
    			}
            else if(col.tag=="Shield")
                {
                    Destroy(gameObject);
                    Destroy(col.gameObject);
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienBuzz1);
                }
    	}
    void OnBecomeInvisible()
    	{
    		Destroy(gameObject);
    	}

    
}
