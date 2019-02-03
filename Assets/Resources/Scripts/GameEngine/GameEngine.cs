using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class GameEngine : NetworkBehaviour {

    public TextMeshProUGUI plrCount;
    [SyncVar] public int playerCount;

    void Start () {
        //Application.targetFrameRate = 144;
    }
	
	void Update () {
        if (isServer) {
            playerCount = NetworkServer.connections.Count;
        }
        plrCount.text = "Players: " + playerCount;
    }
}
