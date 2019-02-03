using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyList : MonoBehaviour {

    public static LobbyList instance;
    public int INDEX = 0;
    void OnEnable() {
        instance = this;

    }

    public void AddPlayer(LobbyPlayerScript plrScript) {
        Debug.Log("[Lobby] Player Added");
        plrScript.gameObject.transform.SetParent(this.transform, false);
        plrScript.gameObject.transform.localPosition = new Vector3(0, 145 - (INDEX * 32), 0);
        INDEX++;
    }
    public void RemovePlayer(string name) {
        Debug.Log("[Lobby] Player Removed");
        foreach (Transform child in transform) {
            if(child.name.Equals(name)) {
                Destroy(child);
                INDEX--;
            }
        }
    }
}
