using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene0Object : MonoBehaviour {
    private static Scene0Object playerInstance;
    void Awake() {
        DontDestroyOnLoad(this);

        if (playerInstance == null) {
            playerInstance = this;
        } else {
            DestroyObject(gameObject);
        }
    }
}