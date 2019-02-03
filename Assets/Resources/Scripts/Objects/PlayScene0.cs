using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScene0 : MonoBehaviour {
    void Start() {
        if (Application.isEditor && (GameObject.Find("Scene 0 Object") == null)) {
            Application.LoadLevel(0);
        }
    }
}