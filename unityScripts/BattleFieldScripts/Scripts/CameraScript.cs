using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour {

    public string presentScene="MainScene";
	public Transform cameraTarget;
	public float cameraSpeed=2.5f;
	public float minX=-8f;
	public float minY=-4f;
	public float maxX=100f;
	public float maxY=100f;
	void FixedUpdate()
		{		
		if (cameraTarget != null) 
			{			
			var newPos = Vector2.Lerp (transform.position,
				             cameraTarget.position,
				             Time.deltaTime * cameraSpeed);			
			var vect3 = new Vector3 (newPos.x, newPos.y, -10f);			
			var clampX = Mathf.Clamp (vect3.x, minX, maxX);
			var clampY = Mathf.Clamp (vect3.y, minY, maxY);			
			transform.position = new Vector3(clampX, clampY, -10f);
			}
		}
}
