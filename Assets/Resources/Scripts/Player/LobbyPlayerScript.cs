using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyPlayerScript : NetworkLobbyPlayer {


    private bool inLobby = false;
    [SyncVar(hook = "OnMyName")]
    public string username;
    void Start() {}

    public override void OnClientEnterLobby() {
        base.OnClientEnterLobby();
        inLobby = true;
        //StartCoroutine(EnteredLobby());
    }

    public void ReadyUp() {
        this.SendReadyToBeginMessage();
        transform.Find("Button").GetComponent<Image>().color = new Color(0f, 1f, 0.1f);
    }
    public override void OnClientExitLobby() {
        LobbyList.instance.RemovePlayer(this.name);
    }


    [Command]
    public void CmdNameChanged(string name) {
        this.username = name;
    }

    void OnMyName(string name) {
        this.username = name;
        GetComponentInChildren<Text>().text = name;
    }

    void SetupLocal() {
        transform.Find("Button").GetComponent<Button>().interactable = true;
        CmdNameChanged(PlayerPrefs.GetString("ProfileName"));
    }
    void SetupOther() {
        transform.Find("Button").GetComponent<Button>().interactable = false;
    }
    void Update() {
        if (inLobby) {

            LobbyList.instance.AddPlayer(this);

            if (isLocalPlayer) {
                SetupLocal();
            } else {
                SetupOther();
            }
            OnMyName(username);

            /*if (!isLocalPlayer) {
                GameObject.Find(gameObject.name + "/Button").GetComponent<Button>().interactable = false;
            } else {
                GameObject.Find(gameObject.name + "/Button").GetComponent<Button>().interactable = true;
            }*/
            inLobby = false;
        }
    }
}
