using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSound : MonoBehaviour
{
    AudioSource AS;
    public ImageEffectBasic imageEffectBasic;
    bool effectEnabled = false;
    public AudioClip clip;
    public AudioClip leaveClip;
    public AudioSource musicSource;
    void Start(){
        AS = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other){
        AS.PlayOneShot(clip);
    }
    void OnTriggerExit(Collider other){
        AS.PlayOneShot(leaveClip);
        effectEnabled = !effectEnabled;
        imageEffectBasic.enabled = effectEnabled;
        if(effectEnabled){
            musicSource.pitch = 1.0f;
        }else{
            musicSource.pitch = .45f;
        }
    }
}
