﻿using FMODUnity;
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
    public World belongsTo;

    private bool isDown;
    private float initialYPosition;
    private StudioEventEmitter[] fmodEmitters;
    private Dictionary<int, bool> objectsApplyingPressure;

    // Start is called before the first frame update
    void Start() {
        isDown = false;
        initialYPosition = transform.localPosition.y;
        fmodEmitters = GetComponents<StudioEventEmitter>();
        objectsApplyingPressure = new Dictionary<int, bool>();
    }

    // Update is called once per frame
    void Update() {
        if (objectsApplyingPressure.ContainsValue(true))
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
        World otherWorld = other.gameObject.GetComponent<WorldChanger>()?.belongsTo ?? belongsTo;

        if (belongsTo == World.BOTH || belongsTo == otherWorld)
            objectsApplyingPressure[other.gameObject.GetInstanceID()] = other.gameObject.GetComponent<InteractableObject>()?.isInteracting ?? false;

        //if (belongsTo == World.BOTH || (belongsTo == World.NORMAL && other.gameObject.layer != LayerMask.NameToLayer("UninteractiveWorld")) || (belongsTo == World.ARCANE && other.gameObject.layer == LayerMask.NameToLayer("UninteractiveWorld")))
        //    objectsApplyingPressure[other.gameObject.GetInstanceID()] = other.gameObject.GetComponent<InteractableObject>()?.isInteracting ?? false;

        //if (other.gameObject.layer != LayerMask.NameToLayer("UninteractiveWorld"))
        //    objectsApplyingPressure.Add(other.gameObject.GetInstanceID(), other.gameObject.GetComponent<InteractableObject>()?.isInteracting ?? false);
    }

    private void OnTriggerStay(Collider other) {
        World otherWorld = other.gameObject.GetComponent<WorldChanger>()?.belongsTo ?? belongsTo;

        if (belongsTo == World.BOTH || belongsTo == otherWorld)
            objectsApplyingPressure[other.gameObject.GetInstanceID()] = other.gameObject.GetComponent<InteractableObject>()?.isInteracting ?? false;
        else
            objectsApplyingPressure.Remove(other.gameObject.GetInstanceID());

        //if (belongsTo == World.BOTH || (belongsTo == World.NORMAL && other.gameObject.layer != LayerMask.NameToLayer("UninteractiveWorld")) || (belongsTo == World.ARCANE && other.gameObject.layer == LayerMask.NameToLayer("UninteractiveWorld")))
        //    objectsApplyingPressure[other.gameObject.GetInstanceID()] = other.gameObject.GetComponent<InteractableObject>()?.isInteracting ?? false;
        //else
        //    objectsApplyingPressure.Remove(other.gameObject.GetInstanceID());

        //else if (belongsTo == World.ARCANE && other.gameObject.layer == LayerMask.NameToLayer("UninteractiveWorld"))
        //    objectsApplyingPressure[other.gameObject.GetInstanceID()] = other.gameObject.GetComponent<InteractableObject>()?.isInteracting ?? false;

    }

    private void OnTriggerExit(Collider other) {
        //if (other.gameObject.layer != LayerMask.NameToLayer("UninteractiveWorld"))
            objectsApplyingPressure.Remove(other.gameObject.GetInstanceID());
    }

}
