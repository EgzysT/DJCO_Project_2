using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    public float depth;
    public float animationDuration;

    private bool isBeingPressured;
    private bool isDown;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start() {
        isBeingPressured = false;
        isDown = false;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (isBeingPressured) {
            if (!isDown) {
                isDown = true;
                LeanTween.cancel(gameObject);
                LeanTween.move(gameObject, new Vector3(transform.position.x, initialPosition.y - depth, transform.position.z), animationDuration).setEase(LeanTweenType.easeOutQuart);
            }
        }
        else {
            if (isDown) {
                isDown = false;
                LeanTween.cancel(gameObject);
                LeanTween.move(gameObject, initialPosition, animationDuration).setEase(LeanTweenType.easeOutQuart);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("ontriggerENTER");
        isBeingPressured = true;
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log("ontriggerEXIT");
        isBeingPressured = false;
    }

}
