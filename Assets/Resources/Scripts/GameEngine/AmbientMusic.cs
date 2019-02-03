using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMusic : MonoBehaviour {

    public AudioClip ambientMusic;

    void Start() {
        GetComponent<AudioSource>().clip = ambientMusic;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
    }
    
}
