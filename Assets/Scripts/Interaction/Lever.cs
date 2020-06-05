using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lever : InteractableObject {

    [Header("Event trigger ID (negative to not trigger)")]
    public int id;

    [Header("Lever Settings")]
    public GameObject movingObject;
    public bool multipleInteractions;
    public float rotationDegrees;
    public float animationDuration;

    private bool isActivated;

    // Start is called before the first frame update
    void Start() {
        isActivated = false;
    }

    private void TriggerActivate() {
        if (id >= 0) {
            GameEvents.instance.InteractableActivate(id);
        }
    }

    private void TriggerDeactivate() {
        if (id >= 0) {
            GameEvents.instance.InteractableDeactivate(id);
        }
    }

    public override void LeftMouseButtonDown() {
        if (LeanTween.isTweening(gameObject))
            return;

        if (isActivated) {
            // Go up (Deactivate)
            LeanTween.rotateAroundLocal(movingObject, Vector3.right, -rotationDegrees, animationDuration)
                .setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(TriggerDeactivate);

            isActivated = false;
        }
        else {
            // Go down (Activate)
            LeanTween.rotateAroundLocal(movingObject, Vector3.right, rotationDegrees, animationDuration)
                .setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(TriggerActivate);

            isActivated = true;

            if (!multipleInteractions)
                Destroy(this);
        }
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
