using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
	public float speed=30;
    Rigidbody2D rb;
    public GameObject blt;
    public bool gotHit;
    public AnimationCurve anim;
    public int gotHitOnce=0;
    bool allowedToShoot=true;
    float shootTimer=0f;
   
    void Start()
    {
       rb=GetComponent<Rigidbody2D>(); 
       gotHit=false;
    }

   
    void FixedUpdate()
    {
        float horzMove=Input.GetAxisRaw("Horizontal");
        rb.velocity=new Vector2(horzMove*speed,0);
        if(!allowedToShoot)
        	{
        		shootTimer+=Time.deltaTime;
        		if(shootTimer>3f)
        			{
        				shootTimer=0f;
        				allowedToShoot=true;
        			}
        	}
    }

    void Update()
    	{
    		if(Input.GetButtonDown("Jump") && allowedToShoot)
    			{
    				Instantiate(blt,transform.position,Quaternion.identity); 
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.getCoin);
                    allowedToShoot=false;

    			}
    		if(Input.GetKey(KeyCode.A) )
    		 StartCoroutine(RunAnimation());	
    	}
    public void showAnimation()
    	{

    		StartCoroutine(RunAnimation());	
    	}
    IEnumerator RunAnimation()
    {
        Vector2 startPos = transform.position;
        for(int k=0;k<2;k++)
        {
        for(float x=0;x < anim.keys[anim.length-1].time;x+=4*Time.deltaTime)
            {
                transform.position= new Vector2(startPos.x+anim.Evaluate(x),startPos.y);
                yield return null;
            }
        }
    }
}
