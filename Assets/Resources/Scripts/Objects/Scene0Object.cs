using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene0Object : MonoBehaviour {
     void Start() {
         DontDestroyOnLoad(this.gameObject);
     }
 }