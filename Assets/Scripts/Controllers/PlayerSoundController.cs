using FMOD;
using FMODUnity;
using System;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Space, Header("FMOD Settings")]

    [EventRef]
    public string walkingNormalSoundEventName = "";
    private FMOD.Studio.EventInstance walkingNormalSound;
    [EventRef]
    public string runningNormalSoundEventName = "";
    private FMOD.Studio.EventInstance runningNormalSound;
    [EventRef]
    public string walkingArcaneSoundEventName = "";
    private FMOD.Studio.EventInstance walkingArcaneSound;
    [EventRef]
    public string runningArcaneSoundEventName = "";
    private FMOD.Studio.EventInstance runningArcaneSound;
    [EventRef]
    public string afraidSoundEventName = "";
    private FMOD.Studio.EventInstance afraidSound;
    [EventRef]
    public string lastBreathSoundEventName = "";
    private FMOD.Studio.EventInstance lastBreathSound;


    private Transform cameraTransform;
    private FMOD.Studio.EventInstance walkingSound;
    private FMOD.Studio.EventInstance runningSound;

    void Start()
    {
        walkingNormalSound = RuntimeManager.CreateInstance(walkingNormalSoundEventName);
        runningNormalSound = RuntimeManager.CreateInstance(runningNormalSoundEventName);
        walkingArcaneSound = RuntimeManager.CreateInstance(walkingArcaneSoundEventName);
        runningArcaneSound = RuntimeManager.CreateInstance(runningArcaneSoundEventName);
        runningArcaneSound = RuntimeManager.CreateInstance(runningArcaneSoundEventName);
        afraidSound = RuntimeManager.CreateInstance(afraidSoundEventName);
        lastBreathSound = RuntimeManager.CreateInstance(lastBreathSoundEventName);

        cameraTransform = Camera.main.transform;


        if (WorldsController.instance.GetCurrentWorld() == World.NORMAL)
        {
            walkingSound = walkingNormalSound;
            runningSound = runningNormalSound;
        }
        else
        {
            walkingSound = walkingArcaneSound;
            runningSound = runningArcaneSound;
        }

        GameEvents.instance.onNormalWorldEnter += (_) => onNormalWorldEnter();
        GameEvents.instance.onArcaneWorldEnter += (_) => onArcaneWorldEnter();
    }

    private void onArcaneWorldEnter()
    {
        walkingSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        walkingSound = walkingArcaneSound;
        runningSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        runningSound = runningArcaneSound;
    }

    private void onNormalWorldEnter()
    {
        walkingSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        walkingSound = walkingNormalSound;
        runningSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        runningSound = runningNormalSound;
    }

    private void Update()
    {
        ATTRIBUTES_3D feet3D = RuntimeUtils.To3DAttributes(transform);
        runningSound.set3DAttributes(feet3D);
        walkingSound.set3DAttributes(feet3D);
    }

    public void PlayWalkSound()
    {
        walkingSound.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE walkingState);
        runningSound.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE runningState);

        if (runningState != FMOD.Studio.PLAYBACK_STATE.STOPPED)
            runningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        if (walkingState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            walkingSound.start();
    }

    public void StopSound()
    {
        walkingSound.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE walkingState);
        runningSound.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE runningState);

        if (walkingState != FMOD.Studio.PLAYBACK_STATE.STOPPED)
            walkingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        if (runningState != FMOD.Studio.PLAYBACK_STATE.STOPPED)
            runningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayRunningSound()
    {
        walkingSound.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE walkingState);
        runningSound.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE runningState);

        if (walkingState != FMOD.Studio.PLAYBACK_STATE.STOPPED)
            walkingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        if (runningState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            runningSound.start();
    }

    public void PlayAfraidSound()
    {
        afraidSound.set3DAttributes(RuntimeUtils.To3DAttributes(cameraTransform));
        lastBreathSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        afraidSound.start();
    }

    public void StopAfraidSound()
    {
        lastBreathSound.set3DAttributes(RuntimeUtils.To3DAttributes(cameraTransform));
        afraidSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        lastBreathSound.start();
    }
}
