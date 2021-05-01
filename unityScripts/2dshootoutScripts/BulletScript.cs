using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	Rigidbody2D rb;
	Quaternion qt;
	public float bulletVel=10f;
	GameObject parentx;
	
	public GameObject Parentx{
		get{ return parentx;}
		set{ parentx=value;}
	}
	void Start () 
		{
		rb=GetComponent<Rigidbody2D>();

		rb.velocity=transform.rotation *  Vector3.right* bulletVel;
		// Debug.Log(rb.velocity.normalized.x);
		
		}
	void getRotation(Quaternion q)
		{
			qt=q;
		}
	void GetParent(GameObject p)
		{
			parentx=p;
		}
	void OnBecameInvisible()
		{
			Destroy(gameObject);
		}
	
	void Update () 
		{
		
			// Debug.Log(rb.velocity.normalized);
		}
	void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject != parentx)
				{
					if(col.gameObject.tag=="Player")
						{
							col.gameObject.GetComponent<PlayerMovement>().TakeDamage(10f);
						}
					else if(col.gameObject.tag=="Enemy" )
						{
							col.gameObject.GetComponent<EnemyMovement>().TakeDamage(10f);
						}
					else if(col.gameObject.tag=="zombie")
						{
							col.gameObject.GetComponent<ZombieMovement>().TakeDamage(10f);
						}
					else if(col.gameObject.tag=="wolflord")
						{
							col.gameObject.GetComponent<WolfLordMovement>().TakeDamage(10f);
						}
					Destroy(gameObject);
				}
		}
	public Vector2 VelocityDir()
		{
			// Debug.Log(rb.velocity.normalized);
			if(rb)
				return rb.velocity.normalized;
			else 
				return Vector2.right;
			// return Vector2.right;
		}
	public bool CheckForFutureCollision(GameObject go)
		{
			if(DetectCollisionWithGO(transform.position,rb.velocity.normalized * 100f,go))
				return true;
			return false;
		}
	bool DetectCollisionWithGO(Vector2 origin,Vector2 dir,GameObject go)
        {            
            RaycastHit2D[] hits=Physics2D.RaycastAll(origin,dir.normalized,dir.magnitude);
            
            foreach(RaycastHit2D hitx in hits)
                {
                    if(hitx.collider.gameObject==go)
                        return true;
                }
            return false;
        }
}
