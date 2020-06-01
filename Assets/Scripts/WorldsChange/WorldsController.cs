using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum World { NORMAL, ARCANE, BOTH }

public class WorldsController : MonoBehaviour {

    public static WorldsController instance;

    World currentWorld;
    private ParticleSystem.EmissionModule dustEffectEmission;
    bool isChangingWorlds;

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

    void Awake() {
        instance = this;
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

        if (Input.GetKeyDown(KeyCode.Q) && !isChangingWorlds) {
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

        if (currentWorld == World.NORMAL) {
            dustEffectEmission.enabled = true;
            dustEffect.Play();
            currentWorld = World.ARCANE;
            GameEvents.instance.ArcaneWorldEnter(transform.position);
        }
        else if (currentWorld == World.ARCANE) {
            dustEffectEmission.enabled = false;
            dustEffect.Stop();
            currentWorld = World.NORMAL;
            GameEvents.instance.NormalWorldEnter(transform.position);
        }
        else {
            throw new Exception("[" + gameObject.name + "] Current World must be NORMAL or ARCANE");
        }
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
}