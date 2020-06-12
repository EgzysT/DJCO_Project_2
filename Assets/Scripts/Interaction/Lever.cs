using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

            //GameObject hint = Instantiate(Resources.Load("Hint") as GameObject, GameObject.FindGameObjectWithTag("UI").transform);
            //hint.GetComponent<HintScript>().SetHintTitle("SMARTASS");
            //hint.GetComponent<HintScript>().SetHintText("I see you can use levers. You are so smart. BIG BRAIN");
            GameManager.createHint("SMARTASS", "I see you can use levers. You are so smart. BIG BRAIN");

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
