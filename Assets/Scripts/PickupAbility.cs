using UnityEngine;

public class PickupAbility : MonoBehaviour
{
    public const float DISTANCE = 3f;
    public const float ROTATION_SPEED = 5;

    //private bool isDragging = false;
    //private Rigidbody draggingObject;
    //private Vector3 positionOffset;
    //private Quaternion rotationOffset;


    //private Quaternion originalRotation;
    //private Vector3 _screenPoint;
    //private Vector3 _offset;

    //[Header("Pick Object")]
    //private Rigidbody pickedItemRB;
    private GameObject holdSpot;
    //private FirstPersonController fpController;
    //private float throwForce = 300;
    //private bool isHolding = false;
    //private Pickupable lastLookedObject;
    //public bool canHold = true;

    //[Header("Rotate Object")]
    //private Vector3 m_TargetAngles;
    //private Vector3 m_FollowAngles;
    //private Vector3 m_FollowVelocity;
    //private Quaternion m_OriginalRotation;
    //public float smooth = 0.2f;







    //private void Start()
    //{
    //    holdSpot = GameObject.Find("HoldSpot");
    //    //fpController = GameObject.Find("Player").GetComponent<FirstPersonController>();
    //}

    //private void Update()
    //{
    //    if (Input.GetButton("Fire1"))
    //    {
    //        if (isDragging)
    //        {

    //            //draggingObject.rotation = transform.rotation;

    //            //var targetPos = transform.position - positionOffset;
    //            //var targetRot = transform.localRotation * rotationOffset;

    //            //Vector3 dir = targetPos - transform.position;
    //            //dir = targetRot * dir;
    //            //var res = dir + transform.position;

    //            ////draggingObject.transform.position = res;
    //            //draggingObject.transform.localRotation = targetRot;

    //            //Vector3 newPos = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
    //            //Vector3 curPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z)) + _offset;
    //            //draggingObject.MovePosition(curPosition + transform.forward * 0.2f);

    //            Vector3 curPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z)) + _offset;
    //            //draggingObject.MovePosition(Vector3.Lerp(draggingObject.transform.position, curPosition, 0.3f));
    //            //draggingObject.MoveRotation(transform.localRotation * rotationOffset);

    //            holdSpot.transform.position = curPosition;

    //        }
    //        else
    //        {
    //            RaycastHit hit;
    //            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f);

    //            if (Vector3.Distance(transform.position, hit.transform.gameObject.transform.position) < DISTANCE)
    //            {
    //                draggingObject = hit.rigidbody;
    //                draggingObject.useGravity = false;
    //                isDragging = true;

    //                holdSpot.transform.position = hit.transform.position;
    //                holdSpot.transform.rotation = hit.transform.rotation;
    //                hit.transform.SetParent(holdSpot.transform);


    //                //draggingObject.transform.SetParent(gameObject.transform);

    //                //positionOffset = transform.position - draggingObject.transform.position;
    //                //rotationOffset = Quaternion.Inverse(transform.localRotation * draggingObject.transform.localRotation);

    //                _screenPoint = Camera.main.WorldToScreenPoint(draggingObject.transform.position);
    //                _offset = draggingObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
    //            }
    //        }
    //    } 
    //    else
    //    {
    //        if (isDragging)
    //        {
    //            isDragging = false;
    //            draggingObject.useGravity = true;

    //            draggingObject.transform.SetParent(null);
    //        }
    //    }
    //}

    //void Update()
    //{
    //    if (isHolding)
    //    {
    //        Carry();
    //        CheckDrop();
    //    }
    //    else if (canHold)
    //    {
    //        Pickup();
    //    }
    //}

    //void Pickup()
    //{
    //    //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
    //    //RaycastHit hit;
    //    //if (Physics.Raycast(ray, out hit))
    //    //{
    //    //    Pickupable p = hit.collider.GetComponent<Pickupable>();

    //    //    if (lastLookedObject != null && lastLookedObject != p)
    //    //        lastLookedObject.TurnOffShader();
    //    //    lastLookedObject = p;

    //    //    if (lastLookedObject != null && hit.distance > DISTANCE)
    //    //    {
    //    //        lastLookedObject.TurnOffShader();
    //    //        lastLookedObject = null;
    //    //        return;
    //    //    }

    //    //    if (p != null)
    //    //    {
    //    //        p.TurnOnShader();

    //    //        if (Input.GetKeyDown(KeyCode.E))
    //    //        {
    //    //            p.TurnOffShader();
    //    //            isHolding = true;
    //    //            pickedItemRB = hit.collider.GetComponent<Rigidbody>();
    //    //            pickedItemRB.isKinematic = true;
    //    //            //pickedItemRB.useGravity = false;
    //    //            //pickedItemRB.detectCollisions = true;
    //    //            //pickedItemRB.transform.position = holdSpot.transform.position;
    //    //            //pickedItemRB.transform.SetParent(holdSpot.transform);

    //    //            // For rotation
    //    //            //m_OriginalRotation = pickedItemRB.transform.localRotation;
    //    //        }
    //    //    }
    //    //}
    //}

    //void CheckDrop()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        Drop();
    //    }
    //}

    //void Drop()
    //{
    //    pickedItemRB.transform.SetParent(null);
    //    //holdingObject.GetComponent<Rigidbody>().useGravity = true;
    //    pickedItemRB.isKinematic = false;
    //    pickedItemRB.velocity = gameObject.GetComponent<Rigidbody>().velocity;
    //    isHolding = false;
    //    pickedItemRB = null;
    //    fpController.canLook = true;
    //}

    //void Carry()
    //{
    //    //holdingObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //    pickedItemRB.angularVelocity = Vector3.zero;

    //    CheckRotate();

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Rigidbody tempRB = pickedItemRB;
    //        Drop();
    //        tempRB.AddForce(Camera.main.transform.forward * throwForce);
    //    }
    //}
    
    //void CheckRotate()
    //{
    //    if (Input.GetKeyDown(KeyCode.R))
    //        fpController.canLook = false;

    //    if (Input.GetKey(KeyCode.R))
    //        RotateObject();

    //    if (Input.GetKeyUp(KeyCode.R))
    //        fpController.canLook = true;
    //}

    //void RotateObject()
    //{
    //    // we make initial calculations from the original local rotation
    //    //pickedItemRB.transform.localRotation = m_OriginalRotation;

    //    // read input from mouse or mobile controls
    //    float rotationX = Input.GetAxis("Mouse X");
    //    float rotationY = Input.GetAxis("Mouse Y");

    //    // e mesmo preciso?
    //    // wrap values to avoid springing quickly the wrong way from positive to negative
    //    if (m_TargetAngles.y > 180)
    //    {
    //        m_TargetAngles.y -= 360;
    //        m_FollowAngles.y -= 360;
    //    }
    //    if (m_TargetAngles.x > 180)
    //    {
    //        m_TargetAngles.x -= 360;
    //        m_FollowAngles.x -= 360;
    //    }
    //    if (m_TargetAngles.y < -180)
    //    {
    //        m_TargetAngles.y += 360;
    //        m_FollowAngles.y += 360;
    //    }
    //    if (m_TargetAngles.x < -180)
    //    {
    //        m_TargetAngles.x += 360;
    //        m_FollowAngles.x += 360;
    //    }

    //    // with mouse input, we have direct control with no springback required.
    //    m_TargetAngles.x += rotationY * ROTATION_SPEED;
    //    m_TargetAngles.y += rotationX * ROTATION_SPEED;

    //    // smoothly interpolate current values to target angles
    //    m_FollowAngles = Vector3.SmoothDamp(m_FollowAngles, m_TargetAngles, ref m_FollowVelocity, smooth);

    //    // update the actual gameobject's rotation
    //    pickedItemRB.transform.localRotation = m_OriginalRotation * Quaternion.Euler(-m_FollowAngles.x, m_FollowAngles.y, 0);
    //}
}
