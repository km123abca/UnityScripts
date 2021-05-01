using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSpawn : MonoBehaviour {

	//x -27 to 41
	//y  -19 to 17 
	int minx=-27;
	int maxx=41;
	int miny=-19;
	int maxy=17;
	public GameObject spotPrefab;
	void Start () 
		{
		
		}
	
	
	void Update () 
		{
		
		}
	public void SpawnARandomSpot()
		{
			int xpos=Random.Range(minx,maxx+1);
			int ypos=Random.Range(miny,maxy+1);
			Instantiate(spotPrefab,new Vector2(xpos,ypos),Quaternion.identity);
		}


}
