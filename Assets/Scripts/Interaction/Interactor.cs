using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactionMaxDistance = 3f;

    private Vector3 screenCenter;

    private CrosshairController crosshairController;
    private CameraController cameraController;

    private InteractableObject currentlyLooking;

    private Camera cam;

    private bool canInteract;

    private void Start()
    {
        canInteract = true;
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
        crosshairController = GameObject.Find("Crosshair").GetComponent<CrosshairController>();
        cameraController = GameObject.Find("CameraHolder").GetComponent<CameraController>();
        cam = Camera.main;
    }

    private void Update()
    {
        if (!canInteract)
            return;

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

            cameraController.CanLook = true;
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
                crosshairController.ShowNormal()    ;
                return;
            }

            if (Input.GetKey(KeyCode.R))
            {
                cameraController.CanLook = false;
                currentlyLooking.PressR();
            }
            else
            {
                cameraController.CanLook = true;
            }
        }
    }

    void CheckRaycast()
    {
        RaycastHit hit;

        // It checks everything except the UninteractiveWorld layer
        if (Physics.Raycast(cam.ScreenPointToRay(screenCenter), out hit, interactionMaxDistance, ~(1 << LayerMask.NameToLayer("UninteractiveWorld"))))
            currentlyLooking = hit.collider.GetComponent<InteractableObject>();
        else
            currentlyLooking = null;
    }

    public void setInteract(bool state) {
        canInteract = state;
    }
}
