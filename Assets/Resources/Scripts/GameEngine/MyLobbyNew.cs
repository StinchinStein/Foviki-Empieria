using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyLobbyNew : MonoBehaviour {

    NetworkClient myClient;
    public bool isHostServer = false;
    public GameObject lobbyPrefab, gamePrefab;

    void Start() {
        
    }

    public void JoinGame() {
        SetupClient();
    }
    public void HostGame() {
        isHostServer = true;
        SetupServer();
        SetupLocalClient();
    }


    // Create a server and listen on a port
    public void SetupServer() {
        NetworkServer.Listen(7777);
    }

    // Create a client and connect to the server port
    public void SetupClient() {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect("raffertynh.com", 7777);
    }

    // Create a local client and connect to the local server
    public void SetupLocalClient() {
        myClient = ClientScene.ConnectLocalServer();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
    }

    void OnConnected(NetworkMessage nMsg) {
        GEController.instance.uiMainMenu.SetActive(false);
        GEController.instance.uiLobbyMenuNew.SetActive(true);

        GameObject plr = Instantiate(lobbyPrefab, Vector3.zero, Quaternion.identity);
        //NetworkServer.AddPlayerForConnection(nMsg.conn, plr);
    }

    void Update() {
        
    }
}
