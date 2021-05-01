using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSpotBehav : MonoBehaviour {

	// public float lifespan=10f;
	public string absorber="Player";
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.tag==absorber)
				{
					GameObject.Find("SpotSpawner").GetComponent<TeleportSpawn>().SpawnARandomSpot();
					Destroy(gameObject);
				}
		}

}
