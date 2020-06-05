using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : EventReactor {

    [Header("Sliding Door Settings")]
    public Vector3 finalPosition;
    public float animationDuration;

    private Vector3 initialPosition;

    // Start is called before the first frame update
    protected override void StartEvent() {
        initialPosition = transform.localPosition;
    }

    public override void Activate() {
        // Open Door
        LeanTween.moveLocal(gameObject, finalPosition, animationDuration)
            .setEase(LeanTweenType.easeInOutCubic);
    }

    public override void Deactivate() {
        // Close Door
        LeanTween.moveLocal(gameObject, initialPosition, animationDuration)
            .setEase(LeanTweenType.easeInOutCubic);
    }
}
