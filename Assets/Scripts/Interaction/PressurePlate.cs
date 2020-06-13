using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent(typeof(StudioEventEmitter))]
public class PressurePlate : EventTrigger {

    [Header("Pressure plate Settings")]
    public GameObject movingObject;
    public float ppFinalYPosition;
    public float animationDuration;

    private bool isDown;
    private float initialYPosition;
    private StudioEventEmitter[] fmodEmitters;
    private List<int> objectsApplyingPressure;

    private bool isInteracting;

    // Start is called before the first frame update
    void Start() {
        isInteracting = true;
        isDown = false;
        initialYPosition = transform.localPosition.y;
        fmodEmitters = GetComponents<StudioEventEmitter>();
        objectsApplyingPressure = new List<int>();
    }

    // Update is called once per frame
    void Update() {
        if (isInteracting)
            return;

        if (objectsApplyingPressure.Count > 0) {
            if (!isDown) {
                isDown = true;
                PlaySound(0);
                LeanTween.cancel(movingObject);
                LeanTween.moveLocalY(movingObject, ppFinalYPosition, animationDuration)
                    .setEase(LeanTweenType.easeOutQuart)
                    .setOnComplete(TriggerActivate);
            }
        }
        else {
            if (isDown) {
                isDown = false;
                PlaySound(1);
                LeanTween.cancel(movingObject);
                LeanTween.moveLocalY(movingObject, initialYPosition, animationDuration)
                    .setEase(LeanTweenType.easeOutQuart)
                    .setOnComplete(TriggerDeactivate);
            }
        }
    }

    private void PlaySound(int id) {
        if (!fmodEmitters[id].IsPlaying())
            fmodEmitters[id].Play();
    }

    private void OnTriggerEnter(Collider other) {
        //Debug.Log("ontriggerENTER");
        objectsApplyingPressure.Add(other.gameObject.GetInstanceID());

        isInteracting = other.gameObject.GetComponent<InteractableObject>()?.isInteracting ?? false;
    }

    private void OnTriggerStay(Collider other) {
        //Debug.Log("ontriggerSTAY");
        isInteracting = other.gameObject.GetComponent<InteractableObject>()?.isInteracting ?? false;
    }

    private void OnTriggerExit(Collider other) {
        //Debug.Log("ontriggerEXIT");
        objectsApplyingPressure.Remove(other.gameObject.GetInstanceID());
        isInteracting = false;
    }

}
