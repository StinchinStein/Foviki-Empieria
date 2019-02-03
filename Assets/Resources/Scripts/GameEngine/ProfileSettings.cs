using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSettings : MonoBehaviour {

    public InputField inpField;
    void Start() {
        if (PlayerPrefs.HasKey("ProfileName")) {
            // masterMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
            inpField.text = PlayerPrefs.GetString("ProfileName");
        }

    }

    public void SaveSettings() {
        PlayerPrefs.SetString("ProfileName", inpField.text);
        GEController.instance.uiProfileSettings.SetActive(false);
        GEController.instance.uiMainMenu.SetActive(true);
    }
    
    void Update() {

        //masterMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        //masterSlider.value = PlayerPrefs.GetFloat("MasterVol");
    }
}
