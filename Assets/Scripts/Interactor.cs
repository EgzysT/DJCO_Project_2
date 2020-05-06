using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor {

    public float interactionMaxDistance = 3f;

    private Vector3 screenCenter;
    private readonly CrosshairController crosshairController;
    private InteractableObject currentlyLooking;
    private readonly MouseLook firstPerson;

    public Interactor() {
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
        crosshairController = GameObject.Find("Crosshair").GetComponent<CrosshairController>();
        firstPerson = GameObject.Find("MainCamera").GetComponent<MouseLook>();
    }

    public void Interact() {
        
        // Check if not holding the object because of distance
        if (currentlyLooking == null || !currentlyLooking.isInteracting)
            CheckRaycast();

        if (currentlyLooking == null)
        {
            crosshairController.ShowNormal();
            return;
        }

        if (!currentlyLooking.isInteracting)
        {
            crosshairController.ShowInteract();

            if (Input.GetMouseButtonDown(0))
            {
                currentlyLooking.LeftMouseButtonDown();
                crosshairController.ShowNone();
            }

            firstPerson.canLook = true;
        } 
        else
        {
            crosshairController.ShowNone();
            currentlyLooking.Interacting();

            if (Input.GetMouseButtonUp(0)) 
            {
                currentlyLooking.LeftMouseButtonUp();
                currentlyLooking = null;
                crosshairController.ShowNormal();
                return;
            }

            if (Input.GetMouseButtonDown(1)) 
            {
                currentlyLooking.RightMouseButtonDown();
                currentlyLooking = null;
                crosshairController.ShowNormal();
                return;
            }

            if (Input.GetKey(KeyCode.R)) 
            {
                firstPerson.canLook = false;
                currentlyLooking.PressR();
            }
            else 
            {
                firstPerson.canLook = true;
            }
        }
    }

    void CheckRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(screenCenter), out hit, interactionMaxDistance)) 
            currentlyLooking = hit.collider.GetComponent<InteractableObject>();
        else
            currentlyLooking = null;
    }
}
