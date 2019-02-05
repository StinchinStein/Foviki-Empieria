using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private Animator anim;
    public bool isOpen;
    void Start() {
        anim = GetComponent<Animator>();
    }
    
    void Update() {
        anim.SetBool("IsOpen", isOpen);

    }
}
