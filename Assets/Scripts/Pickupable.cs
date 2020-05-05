using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public float Distance = 3f;
    public float RotationSpeed = 10f;
    public float ZoomSpeed = 50f;

    public bool canHold = true;
    public bool isHolding = false;

    private Rigidbody rb;
    private GameObject holdSpot;
    private MouseLook firstPerson;

    //private Renderer renderer;
    //private Shader defaultShader;
    //private Shader outlinedShader;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        holdSpot = GameObject.Find("HoldSpot");
        firstPerson = GameObject.Find("MainCamera").GetComponent<MouseLook>();

        //renderer = GetComponent<Renderer>();
        //defaultShader = renderer.material.shader;
        //outlinedShader = Shader.Find("Outlined/UltimateOutline");
    }

    private void Update()
    {
        if (!isHolding)
            return;

        firstPerson.canLook = !Input.GetKey(KeyCode.R);

        Zoom();
    }

    private void OnMouseDown()
    {
        if (!canHold)
            return;

        isHolding = true;
        rb.useGravity = false;
        transform.SetParent(holdSpot.transform);
    }

    private void OnMouseUp()
    {
        isHolding = false;
        rb.useGravity = true;
        transform.SetParent(null);

        // Make sure to free the camera after dropping
        firstPerson.canLook = true;
    }

    private void OnMouseDrag()
    {
        if (!isHolding || !Input.GetKey(KeyCode.R))
            return;

        float xRotation = -Input.GetAxis("Mouse X") * RotationSpeed;
        float yRotation = Input.GetAxis("Mouse Y") * RotationSpeed;

        transform.Rotate(transform.parent.up, xRotation, Space.World);
        transform.Rotate(transform.parent.right, yRotation, Space.World);
    }

    private void Zoom()
    {
        if (!isHolding)
            return;

        float zoom = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;

        transform.Translate(transform.parent.forward * zoom * Time.deltaTime, Space.World);
    }

    private void Throw()
    {

    }

    //public void TurnOnShader()
    //{
    //    renderer.material.shader = outlinedShader;
    //}

    //public void TurnOffShader()
    //{
    //    renderer.material.shader = defaultShader;
    //}

}
