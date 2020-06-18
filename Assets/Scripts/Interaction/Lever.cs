using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(StudioEventEmitter))]
public class Lever : InteractableObject
{

    [Header("Lever Settings")]
    public GameObject movingObject;
    public bool multipleInteractions;
    public float rotationDegrees;
    public float animationDuration;

    private bool isActivated;
    private StudioEventEmitter fmodEmitter;

    // Start is called before the first frame update
    void Start() {
        isActivated = false;
        fmodEmitter = GetComponent<StudioEventEmitter>();
    }

    public override void LeftMouseButtonDown() {
        if (LeanTween.isTweening(gameObject))
            return;

        if (isActivated) {
            // Go up (Deactivate)
            fmodEmitter.Play();
            LeanTween.rotateAroundLocal(movingObject, Vector3.right, -rotationDegrees, animationDuration)
                .setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(TriggerDeactivate);

            isActivated = false;
        }
        else {
            // Go down (Activate)
            fmodEmitter.Play();
            LeanTween.rotateAroundLocal(movingObject, Vector3.right, rotationDegrees, animationDuration)
                .setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(TriggerActivate);

            isActivated = true;

            if (!multipleInteractions) {
                Destroy(this);
                return;
            }
        }
    }

    public override bool OnTimeout() {
        return LeanTween.isTweening(gameObject);
    }

    public override void LeftMouseButtonUp() {
        // Do nothing
    }

    public override void RightMouseButtonDown() {
        // Do nothing
    }

    public override void PressR() {
        // Do nothing
    }

    public override void Interacting() {
        // Do nothing
    }
}
