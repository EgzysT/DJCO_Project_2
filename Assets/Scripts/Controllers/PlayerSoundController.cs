using FMOD;
using FMODUnity;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerSoundController : MonoBehaviour
{
    [Space, Header("FMOD Settings")]

    [EventRef]
    public string walkingSoundEventName = "";
    private FMOD.Studio.EventInstance walkingSound;
    [EventRef]
    public string runningSoundEventName = "";
    private FMOD.Studio.EventInstance runningSound;

    [EventRef]
    public string afraidSoundEventName = "";
    private FMOD.Studio.EventInstance afraidSound;
    [EventRef]
    public string lastBreathSoundEventName = "";
    private FMOD.Studio.EventInstance lastBreathSound;

    private Transform cameraTransform;

    private enum CURRENT_TERRAIN { DIRT, STONE, METAL, WATER, ARCANE };
    private CURRENT_TERRAIN currentTerrain;

    private enum MOVEMENT { WALK, RUN, IDLE }
    private MOVEMENT currentMovement = MOVEMENT.IDLE;

    float timer = 0.0f;
    float footstepSpeed;
    public float walkFootstepSpeed = 0.5f;
    public float runningFootstepSpeed = 0.35f;

    void Start()
    {
        walkingSound = RuntimeManager.CreateInstance(walkingSoundEventName);
        runningSound = RuntimeManager.CreateInstance(runningSoundEventName);

        afraidSound = RuntimeManager.CreateInstance(afraidSoundEventName);
        lastBreathSound = RuntimeManager.CreateInstance(lastBreathSoundEventName);

        cameraTransform = Camera.main.transform;

        lastBreathSound.set3DAttributes(RuntimeUtils.To3DAttributes(cameraTransform));
        afraidSound.set3DAttributes(RuntimeUtils.To3DAttributes(cameraTransform));

        if (WorldsController.instance.GetCurrentWorld() == World.NORMAL)
        {
            DetermineTerrain();
        }
        else
        {
            currentTerrain = CURRENT_TERRAIN.ARCANE;
        }

        GameEvents.instance.onNormalWorldEnter += (_) => onNormalWorldEnter();
        GameEvents.instance.onArcaneWorldEnter += (_) => onArcaneWorldEnter();
    }

    private void Update()
    {
        if (currentTerrain != CURRENT_TERRAIN.ARCANE)
        {
            DetermineTerrain();
        }
        LoopFootsteps();
    }


    private void LoopFootsteps()
    {
        ATTRIBUTES_3D feet3D = RuntimeUtils.To3DAttributes(transform);
        runningSound.set3DAttributes(feet3D);
        walkingSound.set3DAttributes(feet3D);
        if (timer > footstepSpeed)
        {
            PlayFootsteps();
            timer = 0.0f;
        }
        timer += Time.deltaTime;
    }

    private void PlayFootsteps()
    {
        switch (currentMovement)
        {
            case MOVEMENT.WALK:
                walkingSound.start();
                break;

            case MOVEMENT.RUN:
                runningSound.start();
                break;

            default:
                break;
        }
    }

    public void PlayWalkingSound()
    {
        currentMovement = MOVEMENT.WALK;
        footstepSpeed = walkFootstepSpeed;
    }

    public void PlayRunningSound()
    {
        currentMovement = MOVEMENT.RUN;
        footstepSpeed = runningFootstepSpeed;
    }

    public void StopMovementSound()
    {
        currentMovement = MOVEMENT.IDLE;
    }

    public void PlayAfraidSound()
    {
        afraidSound.set3DAttributes(RuntimeUtils.To3DAttributes(cameraTransform));
        lastBreathSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        afraidSound.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE afraidSoundState);
        if(afraidSoundState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            afraidSound.start();
    }

    public void StopAfraidSound()
    {
        //lastBreathSound.set3DAttributes(RuntimeUtils.To3DAttributes(cameraTransform));
        afraidSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //lastBreathSound.start();
    }

    private void DetermineTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 10.0f);

        foreach (RaycastHit rayhit in hit)
        {
            if(rayhit.transform.gameObject.name == "Feet") continue;

            switch (rayhit.transform.tag)
            {
                case "Metal":
                    currentTerrain = CURRENT_TERRAIN.METAL;
                    break;
                case "Stone": default:
                    currentTerrain = CURRENT_TERRAIN.STONE;
                    break;
                case "Dirt":
                    currentTerrain = CURRENT_TERRAIN.DIRT;
                    break;
                case "Water":
                    currentTerrain = CURRENT_TERRAIN.WATER;
                    break;
            }
        }

        walkingSound.setParameterByName("Terrain", (float)currentTerrain);
        runningSound.setParameterByName("Terrain", (float)currentTerrain);
    }

    private void onArcaneWorldEnter()
    {
        currentTerrain = CURRENT_TERRAIN.ARCANE;
        StopMovementSound();
        walkingSound.setParameterByName("Terrain", (float)currentTerrain);
        runningSound.setParameterByName("Terrain", (float)currentTerrain);
    }

    private void onNormalWorldEnter()
    {
        StopMovementSound();
        DetermineTerrain();
    }

    private void StopAllSounds()
    {
        StopMovementSound();
        walkingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        runningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        afraidSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        lastBreathSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void OnDestroy()
    {
        StopAllSounds();
    }
}
