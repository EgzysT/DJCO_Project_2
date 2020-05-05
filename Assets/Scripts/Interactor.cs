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
        
        // Check if not holding the object because of distance
        if (currentlyLooking == null || !currentlyLooking.isHolding)
            CheckRaycast();

        if (currentlyLooking == null)
        {
            crosshairController.ShowNormal();
            return;
        }

        if (!currentlyLooking.isHolding)
        {
            crosshairController.ShowInteract();

            if (Input.GetMouseButtonDown(0))
            {
                currentlyLooking.Pick();
                crosshairController.ShowNone();
            }

            firstPerson.canLook = true;
        } 
        else
        {
            crosshairController.ShowNone();
            currentlyLooking.CheckDistance();
            currentlyLooking.CheckAngle();
            currentlyLooking.Zoom();

            if (Input.GetMouseButtonUp(0)) 
            {
                currentlyLooking.Drop();
                currentlyLooking = null;
                crosshairController.ShowNormal();
                return;
            }

            if (Input.GetMouseButtonDown(1)) 
            {
                currentlyLooking.Throw();
                currentlyLooking = null;
                crosshairController.ShowNormal();
                return;
            }

            if (Input.GetKey(KeyCode.R)) 
            {
                firstPerson.canLook = false;
                currentlyLooking.Rotate();
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
            currentlyLooking = hit.collider.GetComponent<Pickupable>();
        else
            currentlyLooking = null;
    }
}
