using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour {

    private int val = 0;
    public AudioClip alarmSound;
    private bool playing = false;

    void Start () {
        GetComponent<AudioSource>().clip = alarmSound;
        GetComponent<AudioSource>().loop = false;
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        val++;
        if (val % 1500 > 1500/2) {
            GetComponent<Light>().intensity = 0.35f;
            if (!playing) {
                GetComponent<AudioSource>().Play();
                playing = true;
            }
        } else {
            if (playing) {
                //GetComponent<AudioSource>().Stop();
                playing = false;
            }
            GetComponent<Light>().intensity = 0.15f;
        }
    }
}
