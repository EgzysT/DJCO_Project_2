using FMODUnity;
using System;
using UnityEngine;

public enum World { NORMAL, ARCANE, BOTH }

public class WorldsController : MonoBehaviour {

    public static WorldsController instance;

    World currentWorld;
    private ParticleSystem.EmissionModule dustEffectEmission;
    bool isChangingWorlds;

    public bool canChangeWorlds;
    private CrystalIcon crystalIcon;

    [Header("Shader Settings")]
    public ParticleSystem dustEffect;

    [SerializeField]
    float radius;
    private float effectProgressionPercent;
    public float maxRadius;
    public float effectSpeed;
    private float effectSpeedPercent;
    public AnimationCurve effectTransitionCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    private Camera cam;

    [Header("Camera Effects")]
    private bool cameraEffectsActive;
    private float initialFov;
    private float cameraEffectsProgressionPercent;
    public float cameraEffectsDuration;
    public AnimationCurve fovTransitionCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);

    private FMOD.Studio.EventInstance soundTransition;
    private FMOD.Studio.EventInstance soundArcane;

    void Awake() {
        instance = this;

        // Icon
        canChangeWorlds = false;
        crystalIcon = GameObject.FindGameObjectWithTag("CrystalIcon").GetComponent<CrystalIcon>();

        // Sounds
        soundTransition = RuntimeManager.CreateInstance("event:/SFX/Transition");
        soundTransition.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        soundArcane = RuntimeManager.CreateInstance("event:/Ambience/Arcane");
        soundArcane.set3DAttributes(RuntimeUtils.To3DAttributes(transform));

        // Effects
        effectSpeedPercent = effectSpeed / maxRadius;
        cam = Camera.main;
        initialFov = cam.fieldOfView;

        // Starts on the NORMAL world
        currentWorld = World.NORMAL;
        dustEffectEmission = dustEffect.emission;
        dustEffect.Stop();
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Q) && !isChangingWorlds && canChangeWorlds) {
            Shader.SetGlobalVector("_Position", transform.position);
            changeWorlds();
        }

        if (isChangingWorlds)
            UpdateRadius();

        if (cameraEffectsActive)
            UpdateCameraTransitionEffects();

        Shader.SetGlobalFloat("_Radius", radius);
        Shader.SetGlobalFloat("_ScannerRadius", radius);
    }

    void changeWorlds() {
        isChangingWorlds = true;
        cameraEffectsActive = true;

        soundTransition.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        soundTransition.start();

        if (currentWorld == World.NORMAL) {
            dustEffectEmission.enabled = true;
            dustEffect.Play();
            currentWorld = World.ARCANE;
            GameEvents.instance.ArcaneWorldEnter(transform.position);

            // Play Arcane World Ambient Sound
            soundArcane.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
            soundArcane.start();
        }
        else if (currentWorld == World.ARCANE) {
            dustEffectEmission.enabled = false;
            dustEffect.Stop();
            currentWorld = World.NORMAL;
            GameEvents.instance.NormalWorldEnter(transform.position);

            // Stop Arcane World Ambient Sound
            soundArcane.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        else {
            throw new Exception("[" + gameObject.name + "] Current World must be NORMAL or ARCANE");
        }

        crystalIcon.ChangeWorlds(currentWorld, cameraEffectsDuration);
    }

    void UpdateRadius() {
        if (currentWorld == World.NORMAL) {
            // Is changing to NORMAL world => radius decreasing
            if (effectProgressionPercent <= 0f) {
                isChangingWorlds = false;
                effectProgressionPercent = 0f;
            }
            else
                effectProgressionPercent -= effectSpeedPercent * Time.deltaTime;
        } else {
            // Is changing to ARCANE world => radius increasing
            if (effectProgressionPercent >= 1f) {
                isChangingWorlds = false;
                effectProgressionPercent = 1f;
            }
            else
                effectProgressionPercent += effectSpeedPercent * Time.deltaTime;
        }

        radius = effectTransitionCurve.Evaluate(effectProgressionPercent) * maxRadius;
    }

    void UpdateCameraTransitionEffects() {
        if (cameraEffectsProgressionPercent >= 1f) {
            // Curve initial and final values should be the same
            cameraEffectsProgressionPercent = 0f;
            cameraEffectsActive = false;
        } else {
            cameraEffectsProgressionPercent += Time.deltaTime / cameraEffectsDuration;
        }
        
        cam.fieldOfView = initialFov + fovTransitionCurve.Evaluate(cameraEffectsProgressionPercent);
    }

    public World GetCurrentWorld()
    {
        return currentWorld;
    }
}