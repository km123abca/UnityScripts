using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    public static SoundManager Instance =null;
    public AudioClip alienBuzz1,alienBuzz2,alienDies,bulletFire,shipExplosion;
    public AudioClip rockSmash,getCoin,mannyDies,mannyJump;
    private AudioSource soundEffectAudio;
    void Start()
    {
        if(Instance==null)
        	Instance=this;
        else if(Instance!=this)
        	Destroy(gameObject);
        soundEffectAudio=GetComponent<AudioSource>();
     }

    
    public void PlayOneShot(AudioClip clip)
    {
        soundEffectAudio.PlayOneShot(clip);
    }
}
