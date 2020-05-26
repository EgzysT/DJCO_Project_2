using System;
using UnityEngine;

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

    private new Rigidbody rigidbody;
    private new Camera camera;
    private GameObject holdSpot;

    private void Start() 
    {
        rigidbody = GetComponent<Rigidbody>();
        holdSpot = GameObject.Find("HoldSpot");
        camera = Camera.main;

        if (MaxDistance <= MaxZoomDistance || MinDistance >= MinZoomDistance)
            throw new Exception("[" + gameObject.name + "] MinDistance must be < than MinZoomDistance and MaxDistance must be > than MaxZoomDistance");
    }

    private void Pick() 
    {
        isInteracting = true;
        rigidbody.useGravity = false;

        holdSpot.transform.position = transform.position;
        transform.SetParent(holdSpot.transform);
        holdSpot.GetComponent<FixedJoint>().connectedBody = rigidbody;

        rigidbody.freezeRotation = true;
    }

    private void Drop() 
    {
        isInteracting = false;
        rigidbody.useGravity = true;
        rigidbody.detectCollisions = true;

        transform.SetParent(null);
        holdSpot.GetComponent<FixedJoint>().connectedBody = null;

        rigidbody.freezeRotation = false;
    }

    private void Rotate() 
    {
        rigidbody.freezeRotation = false;

        float xRotation = -Input.GetAxis("Mouse X") * RotationSpeed;
        float yRotation = Input.GetAxis("Mouse Y") * RotationSpeed;

        holdSpot.transform.Rotate(camera.transform.up, xRotation, Space.World);
        holdSpot.transform.Rotate(camera.transform.right, yRotation, Space.World);

        rigidbody.freezeRotation = true;
    }

    private void Zoom() 
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        float distance = Mathf.Clamp(Vector3.Distance(holdSpot.transform.position, camera.transform.position), MinZoomDistance, MaxZoomDistance);

        if ((zoom < 0 && distance <= MinZoomDistance) || (zoom > 0 && distance >= MaxZoomDistance))
            return;

        holdSpot.transform.Translate(camera.transform.forward * zoom * Time.deltaTime, Space.World);
    }

    private void Throw() 
    {
        Drop();

        rigidbody.AddForce(camera.transform.forward * ThrowForce, ForceMode.Impulse);
    }

    private void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, camera.transform.position);
        if (distance > MaxDistance || distance < MinDistance)
            Drop();
    }

    private void CheckAngle()
    {
        float angle = Vector3.Angle(camera.transform.forward, transform.position - camera.transform.position);
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
