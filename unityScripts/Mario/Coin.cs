using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Coin" || col.collider.tag == "BrickBlock")
            return;
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.getCoin);
        IncreaseTextUIScore();
        Destroy(gameObject);
    }
    void IncreaseTextUIScore()
    {
        var textUIComp = GameObject.Find("Score").GetComponent<Text>();
        int Score = int.Parse(textUIComp.text) + 10;
        textUIComp.text = Score.ToString();
    }
}
