using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    //FMOD.Studio.EventInstance SFXVolumeTestEvent;

    FMOD.Studio.Bus MasterBus;
    //FMOD.Studio.Bus MusicBus;
    //FMOD.Studio.Bus SFXBus;

    float MasterVolume = 1f;
    //float MusicVolume = 0.5f;
    //float SFXVolume = 0.5f;

    void Awake() {
        //MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/Voice");
        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/Ambience");
    }

    public void SetMasterVolume(float volume) {
        MasterBus.setVolume(volume);
    }
}
