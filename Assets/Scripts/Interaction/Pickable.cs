using FMODUnity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public class Pickable : InteractableObject
{
    [Header("Contraints")]
    public float RotationSpeed = 5f;
    public float ZoomSpeed = 50f;
    public float ThrowForce = 10f;

    public float MinZoomDistance = 1f;
    public float MaxZoomDistance = 3f;
    public float MaxDistance = 3.8f;
    public float MinDistance = 0.8f;
    public float MaxCollisionVelocity = 3.8f;
    public float MaxAngle = 35f;

    private Rigidbody rb;
    private Camera cam;
    private GameObject holdSpot;
    private bool inCollisionTimeout;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        holdSpot = GameObject.Find("HoldSpot");
        cam = Camera.main;

        if (MaxDistance <= MaxZoomDistance || MinDistance >= MinZoomDistance)
            throw new Exception("[" + gameObject.name + "] MinDistance must be < than MinZoomDistance and MaxDistance must be > than MaxZoomDistance");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!inCollisionTimeout)
        {
            RuntimeManager.PlayOneShot("event:/SFX/Collision", transform.position);
            StartCoroutine(StartCollisionTimeout());
        }
    }

    IEnumerator StartCollisionTimeout()
    {
        inCollisionTimeout = true;
        yield return new WaitForSeconds(.25f);
        inCollisionTimeout = false;
    }

    private void Pick() 
    {
        isInteracting = true;
        rb.useGravity = false;

        holdSpot.transform.position = transform.position;
        transform.SetParent(holdSpot.transform);
        holdSpot.GetComponent<FixedJoint>().connectedBody = rb;

        rb.freezeRotation = true;
    }

    private void Drop() 
    {
        isInteracting = false;
        rb.useGravity = true;
        rb.detectCollisions = true;

        transform.SetParent(null);
        holdSpot.GetComponent<FixedJoint>().connectedBody = null;

        rb.freezeRotation = false;
    }

    private void Rotate() 
    {
        rb.freezeRotation = false;

        float xRotation = -Input.GetAxis("Mouse X") * RotationSpeed;
        float yRotation = Input.GetAxis("Mouse Y") * RotationSpeed;

        holdSpot.transform.Rotate(cam.transform.up, xRotation, Space.World);
        holdSpot.transform.Rotate(cam.transform.right, yRotation, Space.World);

        rb.freezeRotation = true;
    }

    private void Zoom() 
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        float distance = Mathf.Clamp(Vector3.Distance(holdSpot.transform.position, cam.transform.position), MinZoomDistance, MaxZoomDistance);

        if ((zoom < 0 && distance <= MinZoomDistance) || (zoom > 0 && distance >= MaxZoomDistance))
            return;

        holdSpot.transform.Translate(cam.transform.forward * zoom * Time.deltaTime, Space.World);
    }

    private void Throw() 
    {
        Drop();

        rb.AddForce(cam.transform.forward * ThrowForce, ForceMode.Impulse);
    }

    private void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, cam.transform.position);
        if (distance > MaxDistance || distance < MinDistance)
            Drop();
    }

    private void CheckAngle()
    {
        float angle = Vector3.Angle(cam.transform.forward, transform.position - cam.transform.position);
        if (angle > MaxAngle)
            Drop();
    }

    // Implement InteractableObject methods
    public override void LeftMouseButtonDown()
    {
        Pick();
    }

    public override void LeftMouseButtonUp()
    {
        Drop();
    }

    public override void RightMouseButtonDown()
    {
        Throw();
    }

    public override void PressR()
    {
        Rotate();
    }

    public override void Interacting()
    {
        CheckDistance();
        CheckAngle();
        Zoom();
    }
}
