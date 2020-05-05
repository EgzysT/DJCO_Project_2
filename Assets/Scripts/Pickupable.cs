using UnityEngine;

public class Pickupable : MonoBehaviour
{
    //public float Distance = 3f;
    public float RotationSpeed = 5f;
    public float ZoomSpeed = 50f;
    public float ThrowForce = 10f;

    public float MinZoomDistance = 1f;
    public float MaxZoomDistance = 3f;

    public bool canHold = true;
    public bool isHolding = false;

    private new Rigidbody rigidbody;
    private new Camera camera;
    private GameObject holdSpot;
    private MouseLook firstPerson;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        holdSpot = GameObject.Find("HoldSpot");
        firstPerson = GameObject.Find("MainCamera").GetComponent<MouseLook>();
        camera = Camera.main;
    }

    private void Update()
    {
        if (!isHolding)
            return;

        firstPerson.canLook = !Input.GetKey(KeyCode.R);

        Zoom();

        if (Input.GetKeyDown(KeyCode.Mouse1))
            Throw();
    }

    private void OnMouseDown()
    {
        Pick();
    }

    private void OnMouseUp()
    {
        Drop();
    }

    private void OnMouseDrag()
    {
        Rotate();
    }

    private void Pick()
    {
        if (!canHold)
            return;

        isHolding = true;
        rigidbody.useGravity = false;

        holdSpot.transform.position = transform.position;
        transform.SetParent(holdSpot.transform);
        holdSpot.GetComponent<FixedJoint>().connectedBody = rigidbody;

        rigidbody.freezeRotation = true;
    }

    private void Drop()
    {
        isHolding = false;
        rigidbody.useGravity = true;
        rigidbody.detectCollisions = true;

        transform.SetParent(null);
        holdSpot.GetComponent<FixedJoint>().connectedBody = null;

        rigidbody.freezeRotation = false;

        // Make sure to free the camera after dropping
        firstPerson.canLook = true;
    }

    private void Rotate()
    {
        if (!isHolding || !Input.GetKey(KeyCode.R))
            return;

        rigidbody.freezeRotation = false;

        float xRotation = -Input.GetAxis("Mouse X") * RotationSpeed;
        float yRotation = Input.GetAxis("Mouse Y") * RotationSpeed;

        holdSpot.transform.Rotate(camera.transform.up, xRotation, Space.World);
        holdSpot.transform.Rotate(camera.transform.right, yRotation, Space.World);

        rigidbody.freezeRotation = true;
    }

    private void Zoom()
    {
        if (!isHolding)
            return;

        float zoom = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        float distance = Mathf.Clamp(Vector3.Distance(holdSpot.transform.position, camera.transform.position), MinZoomDistance, MaxZoomDistance);

        if ((zoom < 0 && distance <= MinZoomDistance) || (zoom > 0 && distance >= MaxZoomDistance))
            return;

        holdSpot.transform.Translate(camera.transform.forward * zoom * Time.deltaTime, Space.World);
    }

    private void Throw()
    {
        if (!isHolding)
            return;

        Drop();

        rigidbody.AddForce(camera.transform.forward * ThrowForce, ForceMode.Impulse);
    }
}
