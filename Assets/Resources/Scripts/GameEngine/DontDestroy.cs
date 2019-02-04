using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {
    private static DontDestroy playerInstance;
    void Awake() {
        DontDestroyOnLoad(this);

        if (playerInstance == null) {
            playerInstance = this;
        } else {
            DestroyObject(gameObject);
        }
    }
}
