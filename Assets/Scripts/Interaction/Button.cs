using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(StudioEventEmitter))]
public class Button : InteractableObject {

    [Header("Button Settings")]
    public GameObject movingObject;
    public bool multipleInteractions;

    public float buttonTimeout;
    public float buttonFinalZPosition;
    public float animationDuration;

    private bool isActivated;
    private StudioEventEmitter fmodEmitter;
    private float initialZPosition;

    // Start is called before the first frame update
    void Start() {
        isActivated = false;
        fmodEmitter = GetComponent<StudioEventEmitter>();
        initialZPosition = transform.localPosition.z;
    }

    public override void LeftMouseButtonDown() {
        if (LeanTween.isTweening(gameObject))
            return;

        if (!isActivated) {
            // Go down (Activate)
            fmodEmitter.Play();
            LeanTween.moveLocalZ(movingObject, buttonFinalZPosition, animationDuration)
                .setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(() => {
                    TriggerActivate();
                    // Only if it allows multiple interactions
                    if (multipleInteractions)
                        StartCoroutine(ButtonTimeout());
                });

            isActivated = true;

            GameManager.createHint("BUTTONS", "I see you can use buttons. You are so smart. BIG BRAIN");

            if (!multipleInteractions) {
                Destroy(this);
                return;
            }
        }
    }

    private IEnumerator ButtonTimeout() {
        yield return new WaitForSeconds(buttonTimeout);

        // Go up (Deactivate)
        fmodEmitter.Play();
        TriggerDeactivate();
        LeanTween.moveLocalZ(movingObject, initialZPosition, animationDuration)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() => {
                isActivated = false;
            });
    }

    public override bool OnTimeout() {
        return isActivated;
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
