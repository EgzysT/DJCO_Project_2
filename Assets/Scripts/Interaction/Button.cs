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
    private StudioEventEmitter[] fmodEmitters;
    private float initialZPosition;

    // Start is called before the first frame update
    void Start() {
        isActivated = false;
        fmodEmitters = GetComponents<StudioEventEmitter>();
        initialZPosition = transform.localPosition.z;
    }

    public override void LeftMouseButtonDown() {
        if (LeanTween.isTweening(gameObject))
            return;

        if (!isActivated) {
            // Go down (Activate)
            PlaySound(0);

            //Play timer sound
            PlaySound(1);

            LeanTween.moveLocalZ(movingObject, buttonFinalZPosition, animationDuration)
                .setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(() => {
                    TriggerActivate();
                    // Only if it allows multiple interactions
                    if (multipleInteractions)
                        StartCoroutine(ButtonTimeout());
                });

            isActivated = true;

            GameManager.CreateHint("BUTTONS", "I see you can use buttons. You are so smart. BIG BRAIN");

            if (!multipleInteractions) {
                Destroy(this);
                return;
            }
        }
    }

    private IEnumerator ButtonTimeout() {
        yield return new WaitForSeconds(buttonTimeout);

        // Go up (Deactivate)
        PlaySound(0);

        // Stop time sound and play end timer sound 
        StopSound(1);
        PlaySound(2);

        TriggerDeactivate();
        LeanTween.moveLocalZ(movingObject, initialZPosition, animationDuration)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() => {
                isActivated = false;
            });
    }

    private void PlaySound(int id) {
        if (!fmodEmitters[id].IsPlaying())
            fmodEmitters[id].Play();
    }

    private void StopSound(int id) {
        if (fmodEmitters[id].IsPlaying())
            fmodEmitters[id].Stop();
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
