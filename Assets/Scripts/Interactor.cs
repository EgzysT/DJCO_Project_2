using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor {

    public float interactionMaxDistance = 3f;

    private Vector3 screenCenter;
    private readonly CrosshairController crosshairController;
    private Pickupable currentlyLooking;
    private readonly MouseLook firstPerson;

    public Interactor() {
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
        crosshairController = GameObject.Find("Crosshair").GetComponent<CrosshairController>();
        firstPerson = GameObject.Find("MainCamera").GetComponent<MouseLook>();
    }

    public void Interact() {

        if (currentlyLooking == null || (currentlyLooking.isHolding == false))
            CheckRaycast();

        if (Input.GetMouseButtonDown(0)) {
            if (currentlyLooking != null) {
                currentlyLooking.Pick();
                crosshairController.ShowInteract();
            }
        }

        if (currentlyLooking != null && currentlyLooking.isHolding) {
            crosshairController.ShowNone();
            currentlyLooking.Zoom();

            if (Input.GetMouseButtonUp(0)) {
                currentlyLooking.Drop();
                currentlyLooking = null;
                crosshairController.ShowNormal();
                return;
            }

            if (Input.GetMouseButtonDown(1)) {
                currentlyLooking.Throw();
                currentlyLooking = null;
                crosshairController.ShowNormal();
                return;
            }

            if (Input.GetKey(KeyCode.R)) {
                firstPerson.canLook = false;
                currentlyLooking.Rotate();
            }
            else {
                firstPerson.canLook = true;
            }
        }
        else {
            firstPerson.canLook = true;

            if (Input.GetMouseButtonDown(0)) {
                CheckRaycast();
            }
        }
    }

    void CheckRaycast() {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(screenCenter), out hit, interactionMaxDistance)) {
            currentlyLooking = hit.collider.GetComponent<Pickupable>();

            if (currentlyLooking != null) {
                crosshairController.ShowInteract();
                return;
            }
        }

        crosshairController.ShowNormal();
    }
}
