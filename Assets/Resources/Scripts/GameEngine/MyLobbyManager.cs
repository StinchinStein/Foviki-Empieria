using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MyLobbyManager : NetworkLobbyManager {
    

    private GEController gameEngine;

    public InputField addressField;
    public Button hostBtn;
    public Button joinBtn;

    private bool isHosting = false;
    private string mapName = "Asylum";

    void Start() {
        gameEngine = GameObject.Find("GameManager").GetComponent<GEController>();
        //this.client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
    }
    
    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
        //gamePlayer.transform.Translate(GameObject.Find("Spawn Position 1").transform.position, Space.World);
        return true;
    }

    /*public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        Debug.Log("Player Added");
        GameObject plr = Instantiate(this.lobbyPlayerPrefab.gameObject, GetStartPosition().position, Quaternion.identity);
        plr.name = "LobbyPlayer_" + playerControllerId;
        NetworkServer.AddPlayerForConnection(conn, plr, playerControllerId);
        Debug.Log("Player Added - " + plr.name);
    }*/

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId) {
        GameObject plr = Instantiate(this.lobbyPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
        plr.name = "LobbyPlayer_" + playerControllerId;
        Debug.Log("Created Lobby Player");
        return plr;
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId) {
        /*GameObject go = Instantiate(spawnPrefabs[m_currentPlayers[conn.connectionId]]);
        NetworkServer.AddPlayerForConnection(conn, go, playerControllerId);

        return go;*/
        return null;
    }

    public override void OnLobbyServerPlayersReady() {
        //ServerStartGame("Asylum");
        ServerChangeScene("Asylum");
        GEController.instance.STATE = "GAME";
    }
    
    public void ServerStartGame(string sceneName) {
        ServerChangeScene(sceneName);
    }

    IEnumerator SpawnPlayers(string sceneName) {
        yield return new WaitForSeconds(0.5f);

        if (sceneName != "Lobby") {
            foreach (NetworkLobbyPlayer lobbySlot in this.lobbySlots) {
                if (!((Object)lobbySlot == (Object)null)) {
                    NetworkIdentity component = lobbySlot.GetComponent<NetworkIdentity>();
                    GameObject gamePlayer = Instantiate(gamePlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
                    //NetworkServer.ReplacePlayerForConnection()
                    NetworkServer.ReplacePlayerForConnection(lobbySlot.connectionToClient, gamePlayer, lobbySlot.playerControllerId);
                    NetworkServer.Destroy(lobbySlot.gameObject);
                    Debug.Log("Spawning Player " + lobbySlot.name + " #" + component.playerControllerId);
                }
            }
        }

    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GameObject.Find("LobbyCamera").SetActive(false);
        //hide load screen?
        bool loaded = SceneManager.SetActiveScene(scene);
        if (loaded) {
            Debug.Log("Scene Loaded & Active - " + scene.name);
        } else {
            Debug.Log("Scene Was Not Loaded - " + scene.name);
        }
        GEController.instance.uiMainMenu.SetActive(false);
        GEController.instance.uiLobbyMenu.SetActive(true);
    }

    public void StartGame(string lvlName) {
        //Show load screen?
        SceneManager.LoadScene(lvlName, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /*
    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
        var cc = lobbyPlayer.GetComponent<ColorControl>();
        var player = gamePlayer.GetComponent<Player>();
        player.myColor = cc.myColor;
        return true;
    }*/

    public void HostGame() {
        isHosting = true;

        GEController.instance.uiMainMenu.SetActive(false);
        GEController.instance.uiLobbyMenuNew.SetActive(true);
        LobbyList.instance.INDEX = 0;
        //gameEngine.uiLobbySettings.SetActive(true);

        //StartGame("Asylum");
        this.StartHost();

        hostBtn.interactable = false;
        joinBtn.interactable = false;
    }
    public override void OnStartServer() {
        lobbyScene = "Lobby";
        base.OnStartServer();
    }
    
    public override void OnStopServer() {
        GEController.instance.STATE = "MAIN_MENU";
        if (SceneManager.GetActiveScene().name.Equals("Lobby")) {
            base.OnStopServer();
            lobbyScene = "";
        } else {
            lobbyScene = "Lobby";
        }
    }

    public void LeaveGame() {
        if (isHosting) {
            isHosting = false;
            this.StopHost();
            Debug.Log("Stopping lobby, you are host.");
        } else {
            this.StopClient();
            Debug.Log("Disconnecting from host");
        }
        GEController.instance.uiMainMenu.SetActive(true);
        GEController.instance.uiLobbyMenuNew.SetActive(false);
        if (GEController.instance.uiInteractionMenu.activeInHierarchy) {
            GEController.instance.uiInteractionMenu.SetActive(false);
        }
        GEController.instance.STATE = "MAIN_MENU";
        hostBtn.interactable = true;
        joinBtn.interactable = true;
    }
    public void JoinGame() {
        isHosting = false;
        GEController.instance.uiMainMenu.SetActive(false);
        GEController.instance.uiLobbyMenuNew.SetActive(true);
        LobbyList.instance.INDEX = 0;
        //gameEngine.uiLobbySettings.SetActive(true);



        //StartGame("Asylum");
        this.networkAddress = "raffertynh.com";
        this.networkPort = 7777;
        this.StartClient();
        hostBtn.interactable = false;
        joinBtn.interactable = false;
    }

    public void SelectMap(string lvlName) {
        this.mapName = lvlName;
    }


    public void BeginMatch() {

    }
    
    void Update() {
        
    }
}
