using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Header("Event react ID (negative to not react)")]
    public int id;

    [Header("Sliding Door Settings")]
    public Vector3 finalPosition;
    public float animationDuration;

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;

        if (id >= 0) {
            GameEvents.instance.onInteractableActivate += OpenDoor;
            GameEvents.instance.onInteractableDeactivate += CloseDoor;
        }
    }

    private void OpenDoor(int id) {
        if (id == this.id) {
            LeanTween.moveLocal(gameObject, finalPosition, animationDuration)
                .setEase(LeanTweenType.easeInOutCubic);
        }
    }

    private void CloseDoor(int id) {
        if (id == this.id) {
            LeanTween.moveLocal(gameObject, initialPosition, animationDuration)
               .setEase(LeanTweenType.easeInOutCubic);
        }
    }

    void OnDestroy() {
        if (id >= 0) {
            GameEvents.instance.onInteractableActivate -= OpenDoor;
            GameEvents.instance.onInteractableDeactivate -= CloseDoor;
        }
    }
}
