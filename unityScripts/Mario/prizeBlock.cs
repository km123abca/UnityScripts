using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class prizeBlock : MonoBehaviour
{
    public AnimationCurve anim;
    public int coinsInBlock = 5;
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.contacts[0].point.y < transform.position.y)
        {
            StartCoroutine(RunAnimation());
            if (coinsInBlock > 0)
            {
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.getCoin);
                IncreaseTextUIScore();
                coinsInBlock -= 1;
            }
        }

    }

    IEnumerator RunAnimation()
    {
        Vector2 startPos = transform.position;
        for (float x = 0; x < anim.keys[anim.length - 1].time; x += Time.deltaTime)
        {
            transform.position = new Vector2(startPos.x, startPos.y + anim.Evaluate(x));
            yield return null;
        }
    }
    void IncreaseTextUIScore()
    {
        var textUIComp = GameObject.Find("Score").GetComponent<Text>();
        int Score = int.Parse(textUIComp.text) + 10;
        textUIComp.text = Score.ToString();
    }
}
