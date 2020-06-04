using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public GameObject MasterObject;
    public GameObject MusicObject;
    public GameObject VoiceObject;
    public GameObject SFXObject;
    public GameObject AmbienceObject;

    FMOD.Studio.Bus MasterBus;
    FMOD.Studio.Bus MusicBus;
    FMOD.Studio.Bus VoiceBus;
    FMOD.Studio.Bus SFXBus;
    FMOD.Studio.Bus AmbienceBus;

    float MasterVolume = 1f;
    float MusicVolume = 0.5f;
    float VoiceVolume = 0.5f;
    float SFXVolume = 0.5f;
    float AmbienceVolume = 0.5f;

    void Awake() {
        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        MusicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        VoiceBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Voice");
        SFXBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        AmbienceBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Ambience");
    }

    void Start() {
        LoadVolumeSettings();

        SetSliderValue(MasterObject, MasterVolume);
        SetSliderValue(MusicObject, MusicVolume);
        SetSliderValue(VoiceObject, VoiceVolume);
        SetSliderValue(SFXObject, SFXVolume);
        SetSliderValue(AmbienceObject, AmbienceVolume);

        SetMasterVolume(MasterVolume);
        SetMusicVolume(MusicVolume);
        SetVoiceVolume(VoiceVolume);
        SetSFXVolume(SFXVolume);
        SetAmbienceVolume(AmbienceVolume);
    }

    private void LoadVolumeSettings() {
        MasterVolume = float.Parse(PlayerPrefs.GetString(MasterObject.name, MasterVolume.ToString()));
        MusicVolume = float.Parse(PlayerPrefs.GetString(MusicObject.name, MusicVolume.ToString()));
        VoiceVolume = float.Parse(PlayerPrefs.GetString(VoiceObject.name, VoiceVolume.ToString()));
        SFXVolume = float.Parse(PlayerPrefs.GetString(SFXObject.name, SFXVolume.ToString()));
        AmbienceVolume = float.Parse(PlayerPrefs.GetString(AmbienceObject.name, AmbienceVolume.ToString()));
    }

    public void SetMasterVolume(float volume) {
        MasterVolume = volume;
        MasterBus.setVolume(volume);
        SetTMPText(MasterObject, volume);
    }
    
    public void SetMusicVolume(float volume) {
        MusicVolume = volume;
        MusicBus.setVolume(volume);
        SetTMPText(MusicObject, volume);
    }
    
    public void SetVoiceVolume(float volume) {
        VoiceVolume = volume;
        VoiceBus.setVolume(volume);
        SetTMPText(VoiceObject, volume);
    }
    
    public void SetSFXVolume(float volume) {
        SFXVolume = volume;
        SFXBus.setVolume(volume);
        SetTMPText(SFXObject, volume);
    }
    
    public void SetAmbienceVolume(float volume) {
        AmbienceVolume = volume;
        AmbienceBus.setVolume(volume);
        SetTMPText(AmbienceObject, volume);
    }
    private void SetSliderValue(GameObject obj, float value) {
        obj.transform.GetChild(0).GetComponent<Slider>().value = value;
    }

    private void SetTMPText(GameObject obj, float value) {
        obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (int)(value * 100) + "%";

        // Save volume settings
        PlayerPrefs.SetString(obj.name, value.ToString());
    }

}
