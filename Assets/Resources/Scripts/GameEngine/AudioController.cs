using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{

    public AudioMixer masterMixer;
    public Slider masterSlider, musicSlider;

    void Start() {
        masterMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        masterMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        masterSlider.value = PlayerPrefs.GetFloat("MasterVol");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        Debug.Log("[AudioController] Initialized");
    }
    public void SetMasterVol(float val) {
        masterMixer.SetFloat("MasterVol", val);
        PlayerPrefs.SetFloat("MasterVol", val);
    }
    public void SetMusicVol(float val) {
        masterMixer.SetFloat("MusicVol", val);
        PlayerPrefs.SetFloat("MusicVol", val);
    }
}
