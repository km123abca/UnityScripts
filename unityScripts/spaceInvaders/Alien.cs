using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public bool gotHit;
    public string welcomeText;
    public float speed=10;
    public Rigidbody2D rb;
    public Sprite startingImage;
    public Sprite altImage;
    private SpriteRenderer sr;
    public float  secBeforeSpriteChange=2f;
    public GameObject alienBullet;
    public float minFireRateTime=1.0f;
    public float maxFireRateTime=3.0f;
    public float baseFireWaitTime=3.0f;
    public Sprite explodedShipImage;
    void Start()
    {
        gotHit=false;
        welcomeText="Im an alien";
        rb=GetComponent<Rigidbody2D>();
        sr=GetComponent<SpriteRenderer>();
        rb.velocity=Vector2.right * speed;
        StartCoroutine(ChangeAlienSprite());
        baseFireWaitTime=baseFireWaitTime+Random.Range(minFireRateTime,maxFireRateTime);
    }

    public IEnumerator ChangeAlienSprite()
    	{
    	  while(true)
    	  	{  if(!gotHit)
    	  		{
    	  		if(sr.sprite==startingImage)
    	  			sr.sprite=altImage;
    	  		else
    	  			sr.sprite=startingImage;
    	  		}
    	  		yield return new WaitForSeconds(secBeforeSpriteChange);
    	  	}
    	}

    void Turn(int direction)
    	{
    		Vector2 newVelocity=rb.velocity;
    		newVelocity.x=speed*direction;
    		rb.velocity=newVelocity;
    	}
    void moveDown()
    	{
    		Vector2 position=transform.position;
    		position.y-=1;
    		transform.position=position;
    	}
    void OnCollisionEnter2D(Collision2D col)
    	{
    		if(col.gameObject.name=="LeftWall")
    			{
    				Turn(1);
    				moveDown();
    			}
    		else if(col.gameObject.name=="RightWall")
    			{
    				Turn(-1);
    				moveDown();
    			}
    		
    	}
    void OnTriggerEnter2D(Collider2D col)
    	{
    		if(col.gameObject.tag=="Player")
    			{
    				col.GetComponent<SpriteRenderer>().sprite=explodedShipImage;
                    
                    if(! col.GetComponent<SpaceShip>().gotHit)
                        {
        				SoundManager.Instance.PlayOneShot(SoundManager.Instance.shipExplosion);
                        // Destroy(gameObject);                                           
                        col.GetComponent<Alien>().gotHit=true;
                        DestroyObject(col.gameObject,0.5f);
                        }
    			}
    	}

    void FixedUpdate()
        {
            if(Time.time > baseFireWaitTime)
                {
                     baseFireWaitTime=baseFireWaitTime+Random.Range(minFireRateTime,maxFireRateTime);
                      Instantiate(alienBullet,transform.position,Quaternion.identity);
                }
           
        }
}
