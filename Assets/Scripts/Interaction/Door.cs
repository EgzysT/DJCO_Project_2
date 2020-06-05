using System;
using UnityEngine;

public class Door : InteractableObject
{
    [Header("Event react ID (negative to not react)")]
    public int id;

    [Header("Door Settings")]
    public float ThrowForce = 5f;
    public float MaxDistance = 3.8f;
    public float MinDistance = 0f;

    private Rigidbody rb;
    private Camera cam;
    private Vector3 middleScreen;

    private void Start()
    {
        if (id >= 0) {
            GameEvents.instance.onInteractableActivate += OpenDoor;
            GameEvents.instance.onInteractableDeactivate += CloseDoor;
        }

        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        middleScreen = new Vector3(0.5f, 0.5f, 0);
    }

    private void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, cam.transform.position);
        if (distance > MaxDistance || distance < MinDistance)
            isInteracting = false;
    }

    private void Drag()
    {
        Ray playerAim = cam.ViewportPointToRay(middleScreen);

        Vector3 currPos = transform.position;
        Vector3 nextPos = cam.transform.position + playerAim.direction * 2f;
        rb.velocity = (nextPos - currPos) * 10f;
    }

    // Implement InteractableObject methods
    public override void LeftMouseButtonDown()
    {
        isInteracting = true;     
    }

    public override void LeftMouseButtonUp()
    {
        isInteracting = false;
    }

    public override void RightMouseButtonDown() 
    {
        isInteracting = false;
        rb.AddForce(cam.transform.forward * ThrowForce, ForceMode.Impulse);
    }

    public override void Interacting()
    {
        CheckDistance();
        Drag();
    }

    public override void PressR() { }

    private void OpenDoor(int id) {
        if (id == this.id) {
            // TODO Open Door
        }
    }
    
    private void CloseDoor(int id) {
        if (id == this.id) {
            // TODO Close Door
        }
    }

    void OnDestroy() {
        if (id >= 0) {
            GameEvents.instance.onInteractableActivate -= OpenDoor;
            GameEvents.instance.onInteractableDeactivate -= CloseDoor;
        }
    }
}
