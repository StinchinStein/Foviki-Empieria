using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

    public float maxWaitTime, minWaitTime;
    private bool flash = false;
    void Start () {
        StartCoroutine(Flashing());
    }
	
    IEnumerator Flashing() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            flash = !flash;
            if(flash) {
                GetComponent<Light>().intensity = 0.45f;
            } else {
                GetComponent<Light>().intensity = 0.425f;
            }
        }
    }
}
