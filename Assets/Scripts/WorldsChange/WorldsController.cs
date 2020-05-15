using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum World { NORMAL, ARCANE, BOTH }

public class WorldsController : MonoBehaviour {

    public static WorldsController instance;

    World currentWorld;
    bool isChangingWorlds;

    public ParticleSystem dustEffect;

    [SerializeField]
    float radius;
    private float radiusPercent;
    public float maxRadius;
    public float effectSpeed;
    private float effectSpeedPercent;
    public AnimationCurve effectTransitionCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    void Awake() {
        instance = this;
        effectSpeedPercent = effectSpeed / maxRadius;

        // Starts on the NORMAL world
        currentWorld = World.NORMAL;
        dustEffect.Stop();
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Q) && !isChangingWorlds) {
            Shader.SetGlobalVector("_Position", transform.position);
            changeWorlds();
        }

        if (isChangingWorlds) {
            UpdateRadius();
        }
        
        Shader.SetGlobalFloat("_Radius", radius);
    }

    void changeWorlds() {
        isChangingWorlds = true;

        if (currentWorld == World.NORMAL) {
            dustEffect.Play();
            currentWorld = World.ARCANE;
            GameEvents.instance.ArcaneWorldEnter(transform.position);
        }
        else if (currentWorld == World.ARCANE) {
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
            if (radiusPercent <= 0f) {
                isChangingWorlds = false;
                radiusPercent = 0f;
            }
            else
                radiusPercent -= effectSpeedPercent * Time.deltaTime;
        } else {
            // Is changing to ARCANE world => radius increasing
            if (radiusPercent >= 1f) {
                isChangingWorlds = false;
                radiusPercent = 1f;
            }
            else
                radiusPercent += effectSpeedPercent * Time.deltaTime;
        }

        radius = effectTransitionCurve.Evaluate(radiusPercent) * maxRadius;
    }
}