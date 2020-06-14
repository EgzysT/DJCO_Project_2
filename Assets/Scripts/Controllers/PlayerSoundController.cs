using FMOD;
using FMODUnity;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerSoundController : MonoBehaviour
{
    [Space, Header("FMOD Settings")]

    [EventRef]
    public string walkingMetalSoundEventName = "";
    private FMOD.Studio.EventInstance walkingMetalSound;
    [EventRef]
    public string runningMetalSoundEventName = "";
    private FMOD.Studio.EventInstance runningMetalSound;

    [EventRef]
    public string walkingArcaneSoundEventName = "";
    private FMOD.Studio.EventInstance walkingArcaneSound;
    [EventRef]
    public string runningArcaneSoundEventName = "";
    private FMOD.Studio.EventInstance runningArcaneSound;

    [EventRef]
    public string walkingDirtSoundEventName = "";
    private FMOD.Studio.EventInstance walkingDirtSound;
    [EventRef]
    public string runningDirtSoundEventName = "";
    private FMOD.Studio.EventInstance runningDirtSound;

    [EventRef]
    public string walkingStoneSoundEventName = "";
    private FMOD.Studio.EventInstance walkingStoneSound;
    [EventRef]
    public string runningStoneSoundEventName = "";
    private FMOD.Studio.EventInstance runningStoneSound;


    [EventRef]
    public string afraidSoundEventName = "";
    private FMOD.Studio.EventInstance afraidSound;
    [EventRef]
    public string lastBreathSoundEventName = "";
    private FMOD.Studio.EventInstance lastBreathSound;


    private Transform cameraTransform;
    private FMOD.Studio.EventInstance walkingSound;
    private FMOD.Studio.EventInstance runningSound;

    private enum CURRENT_TERRAIN { METAL, STONE, DIRT, ARCANE};

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    void Start()
    {
        walkingMetalSound = RuntimeManager.CreateInstance(walkingMetalSoundEventName);
        runningMetalSound = RuntimeManager.CreateInstance(runningMetalSoundEventName);

        walkingArcaneSound = RuntimeManager.CreateInstance(walkingArcaneSoundEventName);
        runningArcaneSound = RuntimeManager.CreateInstance(runningArcaneSoundEventName);

        walkingDirtSound = RuntimeManager.CreateInstance(walkingDirtSoundEventName);
        runningDirtSound = RuntimeManager.CreateInstance(runningDirtSoundEventName);
        
        walkingStoneSound = RuntimeManager.CreateInstance(walkingStoneSoundEventName);
        runningStoneSound = RuntimeManager.CreateInstance(runningStoneSoundEventName);


        afraidSound = RuntimeManager.CreateInstance(afraidSoundEventName);
        lastBreathSound = RuntimeManager.CreateInstance(lastBreathSoundEventName);

        cameraTransform = Camera.main.transform;


        if (WorldsController.instance.GetCurrentWorld() == World.NORMAL)
        {
            walkingSound = walkingMetalSound;
            runningSound = runningMetalSound;
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
        currentTerrain = CURRENT_TERRAIN.ARCANE;
        
        walkingSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        walkingSound = walkingArcaneSound;
        runningSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        runningSound = runningArcaneSound;
    }

    private void onNormalWorldEnter()
    {
        DetermineTerrain();

        walkingSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        walkingSound = walkingMetalSound;
        runningSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        runningSound = runningMetalSound;
    }

    private void Update()
    {
        ATTRIBUTES_3D feet3D = RuntimeUtils.To3DAttributes(transform);
        runningSound.set3DAttributes(feet3D);
        walkingSound.set3DAttributes(feet3D);

        if (currentTerrain != CURRENT_TERRAIN.ARCANE)
        {
            DetermineTerrain();
        }
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

    public void SelectAndPlayFootstep()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.METAL:
                PlayFootstep(0);
                break;

            case CURRENT_TERRAIN.DIRT:
                PlayFootstep(1);
                break;

            case CURRENT_TERRAIN.STONE:
                PlayFootstep(2);
                break;

            case CURRENT_TERRAIN.ARCANE:
                PlayFootstep(3);
                break;

            default:
                PlayFootstep(0);
                break;
        }
    }

    private void PlayFootstep(int terrain)
    {
        walkingSound.setParameterByName("Terrain", terrain);
        walkingSound.start();
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

    private void DetermineTerrain()
    {
        

        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 10.0f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer != LayerMask.NameToLayer("Ground")) continue;

            switch (rayhit.transform.tag)
            {
                case "Metal":
                    if (currentTerrain != CURRENT_TERRAIN.METAL)
                    {
                        walkingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        walkingSound = walkingMetalSound;
                        runningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        runningSound = runningMetalSound;
                    }
                    currentTerrain = CURRENT_TERRAIN.METAL;
                    break;
                case "Stone":
                    if (currentTerrain != CURRENT_TERRAIN.STONE)
                    {
                        walkingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        walkingSound = walkingStoneSound;
                        runningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        runningSound = runningStoneSound;
                    }
                    currentTerrain = CURRENT_TERRAIN.STONE;
                    break;
                case "Dirt":
                    if (currentTerrain != CURRENT_TERRAIN.DIRT)
                    {
                        walkingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        walkingSound = walkingDirtSound;
                        runningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        runningSound = runningDirtSound;
                    }
                    currentTerrain = CURRENT_TERRAIN.DIRT;
                    break;
            }
            return;
        }
    }
}
