using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnPosIcon : MonoBehaviour {

    void Start() {
        
    }
    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "SpawnIcon.png", true);
    }
    void Update() {
        
    }
}
