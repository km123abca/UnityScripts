using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed=30;
    public Sprite explodedShipImage;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        rb.velocity=-Vector2.up * speed;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    	{
    		if(col.tag=="Wall")
    			{
    				Destroy(gameObject);
    			}
    		else if(col.tag=="Player")
    			{

                    if(col.GetComponent<SpaceShip>().gotHitOnce < 20)
                        {
                        col.GetComponent<SpaceShip>().gotHitOnce+=1;
                        Destroy(gameObject);  
                        col.GetComponent<SpaceShip>().showAnimation();
                        SoundManager.Instance.PlayOneShot(SoundManager.Instance.mannyJump);
                        }
                    else
                        {
                          col.GetComponent<SpriteRenderer>().sprite=explodedShipImage; 

                        if(! col.GetComponent<SpaceShip>().gotHit)
                            {
                            SoundManager.Instance.PlayOneShot(SoundManager.Instance.shipExplosion);
                            Destroy(gameObject);                                           
                            col.GetComponent<SpaceShip>().gotHit=true;
                            DestroyObject(col.gameObject,0.5f);
                            }
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
