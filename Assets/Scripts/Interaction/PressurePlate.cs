using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PressurePlate : EventTrigger {

    [Header("Pressure plate Settings")]
    public float depth;
    public float animationDuration;

    private bool isDown;
    private Vector3 initialPosition;
    private List<int> objectsApplyingPressure;

    // Start is called before the first frame update
    void Start() {
        isDown = false;
        initialPosition = transform.position;
        objectsApplyingPressure = new List<int>();
    }

    // Update is called once per frame
    void Update() {
        if (objectsApplyingPressure.Count > 0) {
            if (!isDown) {
                isDown = true;
                LeanTween.cancel(gameObject);
                LeanTween.move(gameObject, new Vector3(transform.position.x, initialPosition.y - depth, transform.position.z), animationDuration)
                    .setEase(LeanTweenType.easeOutQuart)
                    .setOnComplete(TriggerActivate);
            }
        }
        else {
            if (isDown) {
                isDown = false;
                TriggerDeactivate();
                LeanTween.cancel(gameObject);
                LeanTween.move(gameObject, initialPosition, animationDuration)
                    .setEase(LeanTweenType.easeOutQuart);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        //Debug.Log("ontriggerENTER");
        objectsApplyingPressure.Add(other.gameObject.GetInstanceID());
    }

    private void OnTriggerExit(Collider other) {
        //Debug.Log("ontriggerEXIT");
        objectsApplyingPressure.Remove(other.gameObject.GetInstanceID());
    }

}
