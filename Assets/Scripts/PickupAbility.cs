using UnityEngine;

public class PickupAbility : MonoBehaviour
{
    public const float DISTANCE = 3f;
    public const float ROTATION_SPEED = 5;

    [Header("Pick Object")]
    private Rigidbody pickedItemRB;
    private GameObject holdSpot;
    private FirstPersonController fpController;
    private float throwForce = 300;
    private bool isHolding = false;
    public bool canHold = true;

    [Header("Rotate Object")]
    private Vector3 m_TargetAngles;
    private Vector3 m_FollowAngles;
    private Vector3 m_FollowVelocity;
    private Quaternion m_OriginalRotation;
    public float smooth = 0.2f;

    private void Start()
    {
        holdSpot = GameObject.Find("HoldSpot");
        fpController = GameObject.Find("Player").GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHolding)
            {
                Drop();
            }
            else if (canHold)
            {
                Pickup();
            }
        }
        Carry();
    }

    void Pickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.distance > DISTANCE) return;
                
                Pickupable p = hit.collider.GetComponent<Pickupable>();

                if (p != null)
                {
                    isHolding = true;
                    pickedItemRB = hit.collider.GetComponent<Rigidbody>();
                    pickedItemRB.isKinematic = true;
                    //holdingObject.GetComponent<Rigidbody>().useGravity = false;
                    //holdingObject.GetComponent<Rigidbody>().detectCollisions = true;
                    pickedItemRB.transform.position = holdSpot.transform.position;
                    pickedItemRB.transform.SetParent(holdSpot.transform);

                    // For rotation
                    m_OriginalRotation = pickedItemRB.transform.localRotation;
                }
            }
        }
    }

    void Drop()
    {
        pickedItemRB.transform.SetParent(null);
        //holdingObject.GetComponent<Rigidbody>().useGravity = true;
        pickedItemRB.isKinematic = false;
        pickedItemRB.velocity = gameObject.GetComponent<Rigidbody>().velocity;
        isHolding = false;
        pickedItemRB = null;
        fpController.canLook = true;
    }

    void Carry()
    {
        if (isHolding)
        {
            //holdingObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            pickedItemRB.angularVelocity = Vector3.zero;

            Rotate();

            if (Input.GetMouseButtonDown(0))
            {
                Rigidbody tempRB = pickedItemRB;
                Drop();
                tempRB.AddForce(Camera.main.transform.forward * throwForce);
            }
        }
    }
    
    void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.R))
            fpController.canLook = false;

        if (Input.GetKey(KeyCode.R))
            RotateObject();

        if (Input.GetKeyUp(KeyCode.R))
            fpController.canLook = true;
    }

    void RotateObject()
    {
        // we make initial calculations from the original local rotation
        pickedItemRB.transform.localRotation = m_OriginalRotation;

        // read input from mouse or mobile controls
        float rotationX = Input.GetAxis("Mouse X");
        float rotationY = Input.GetAxis("Mouse Y");

        // e mesmo preciso?
        // wrap values to avoid springing quickly the wrong way from positive to negative
        if (m_TargetAngles.y > 180)
        {
            m_TargetAngles.y -= 360;
            m_FollowAngles.y -= 360;
        }
        if (m_TargetAngles.x > 180)
        {
            m_TargetAngles.x -= 360;
            m_FollowAngles.x -= 360;
        }
        if (m_TargetAngles.y < -180)
        {
            m_TargetAngles.y += 360;
            m_FollowAngles.y += 360;
        }
        if (m_TargetAngles.x < -180)
        {
            m_TargetAngles.x += 360;
            m_FollowAngles.x += 360;
        }

        // with mouse input, we have direct control with no springback required.
        m_TargetAngles.x += rotationY * ROTATION_SPEED;
        m_TargetAngles.y += rotationX * ROTATION_SPEED;

        // smoothly interpolate current values to target angles
        m_FollowAngles = Vector3.SmoothDamp(m_FollowAngles, m_TargetAngles, ref m_FollowVelocity, smooth);

        // update the actual gameobject's rotation
        pickedItemRB.transform.localRotation = m_OriginalRotation * Quaternion.Euler(-m_FollowAngles.x, m_FollowAngles.y, 0);
    }
}
