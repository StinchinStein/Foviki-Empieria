using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoorScript : MonoBehaviour {

	private Animator animator;
	private AudioSource aSource;
	private bool isOpen = false;
	private bool isHovering = false;
	private int cooldown = 0;
	private int clearanceLevel = 0;

	void Start () {
		animator = gameObject.GetComponent<Animator>();
		aSource = gameObject.GetComponent<AudioSource>();
	}


	void OnGUI() {
		if (isHovering == gameObject.transform) {
			string title = "Interaction: " + (isOpen?"Close Door":"Open Door");
			string desc = "<color=blue>Clearance Level: " + clearanceLevel + "</color>";
			GUI.Box (new Rect ((Screen.width - getTextWidth(title)) / 2, ((Screen.height) - 105), getTextWidth(title), 25), title);
			GUI.Box (new Rect ((Screen.width - getTextWidth(desc)) / 2, ((Screen.height) - 75), getTextWidth(desc), 25), desc);
		}

		//Move this into the player class, as this will render EVERY DOOR
		GUI.Label(new Rect ((Screen.width - getTextWidth("*")) / 2, ((Screen.height / 2) - 10), getTextWidth("*"), 25), "*");
	}

	void Update () {
		cooldown++;
		isHovering = false;
		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			Transform col = hit.collider.transform;

			if (col.parent != null && gameObject.transform == col.parent.parent) {
				if (col.parent.tag == "Interactable" || col.parent.tag == "Tree") {
					isHovering = col.parent.parent;

					if (Input.GetKeyDown (KeyCode.F) && cooldown > 50) {
						interaction ("toggle");
						cooldown = 0;
					}
				}
			}
		}
	}

	void interaction(string type) {
		switch (type) {
		case "toggle":
				isOpen ^= true;
				animator.SetBool ("IsOpen", isOpen);
				aSource.PlayDelayed(0.2f);
			break;
		}
	}

	float getTextWidth(string str) {
		return GUI.skin.label.CalcSize(new GUIContent(str)).x + 10;
	}
}
