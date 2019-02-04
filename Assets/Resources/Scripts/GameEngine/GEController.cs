using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GEController : MonoBehaviour {
    
    public static GEController instance;
    public string STATE = "MAIN_MENU";
    public GameObject uiInteractionMenu;
    public GameObject uiLobbySettings;
    public GameObject uiMainMenu;
    public GameObject uiLobbyMenu;
    public GameObject uiLobbyMenuNew;
    public GameObject uiPlayerList;
    public GameObject uiProfileSettings;

    public bool isPaused = false;
    private bool connectingToHost = false;
    
    void Start() {
        instance = this;
        //Debug.Log("Connected!");
        Debug.Log("[GEController] Initialized");
        if(!PlayerPrefs.HasKey("ProfileName")) {
            uiProfileSettings.SetActive(true);
        }
        
    }

    public void OptionsMenu () {
        uiProfileSettings.SetActive(true);
    }
    //Interaction Menu Actions
    public void ResumeGame() {
        uiInteractionMenu.SetActive(false);
        isPaused = false;
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && STATE != "MAIN_MENU") {
            if (!isPaused) {
                //GameObject.Find("FPSController(Clone)").GetComponent<FirstPersonController>().GetComponent<MouseLook>().SetCursorLock(false);
                uiInteractionMenu.SetActive(true);
                isPaused = true;
            } else {
                //GameObject.Find("FPSController(Clone)").GetComponent<FirstPersonController>().GetComponent<MouseLook>().SetCursorLock(true);
                uiInteractionMenu.SetActive(false);
                isPaused = false;
            }
        } else if(STATE == "MAIN_MENU" && uiInteractionMenu.activeInHierarchy) {
            uiInteractionMenu.SetActive(false);
        }
    }
}
