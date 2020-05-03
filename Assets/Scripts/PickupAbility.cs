using UnityEngine;

public class PickupAbility : MonoBehaviour
{
    float throwForce = 300;
    Vector3 objectPos;
    public const float DISTANCE = 3f;

    public bool canHold = true;
    public bool isHolding = false;
    private Rigidbody pickedItemRB;
    private GameObject holdSpot;

    private void Start()
    {
        holdSpot = GameObject.Find("HoldSpot");
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
    }

    void Carry()
    {
        if (isHolding)
        {
            //holdingObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            pickedItemRB.angularVelocity = Vector3.zero;

            if (Input.GetMouseButtonDown(0))
            {
                Rigidbody tempRB = pickedItemRB;
                Drop();
                tempRB.AddForce(Camera.main.transform.forward * throwForce);
            }
        }
    }
}
