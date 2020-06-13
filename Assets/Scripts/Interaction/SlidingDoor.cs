using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : EventReactor {

    [Header("Sliding Door Settings")]
    public Vector3 finalPosition;
    public float animationDuration;

    private Vector3 initialPosition;
    private StudioEventEmitter fmodEmitter;
    private bool isOpen;

    // Start is called before the first frame update
    protected override void StartEvent() {
        initialPosition = transform.localPosition;
        fmodEmitter = GetComponent<StudioEventEmitter>();
    }

    public override void Activate() {
        //if (isOpen) return;
        if (!isOpen)
            fmodEmitter.Play();

        isOpen = true;

        // Open Door
        LeanTween.moveLocal(gameObject, finalPosition, animationDuration)
            .setEase(LeanTweenType.easeInOutCubic);

    }

    public override void Deactivate() {
        //if (!isOpen) return;
        if (isOpen)
            fmodEmitter.Play();

        isOpen = false;

        // Close Door
        LeanTween.moveLocal(gameObject, initialPosition, animationDuration)
            .setEase(LeanTweenType.easeInOutCubic);
    }
}
