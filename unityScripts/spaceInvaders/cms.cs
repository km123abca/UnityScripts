using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class cms
{
    public static void increaseScore(int x)
    	{
    		 var textUIComp = GameObject.Find("Score").GetComponent<Text>();
        	 int Score = int.Parse(textUIComp.text) + x;
        	 textUIComp.text = Score.ToString();
    	}
}
