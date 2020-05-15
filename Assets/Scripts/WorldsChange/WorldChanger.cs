using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shader))]
public class WorldChanger : MonoBehaviour
{
    public World belongsTo;
    private Pickable pickableObject;

    // Start is called before the first frame update
    void Start()
    {
        pickableObject = GetComponent<Pickable>();

        // Alter layer at the start
        if (belongsTo == World.ARCANE)
            gameObject.layer = LayerMask.NameToLayer("UninteractiveWorld");
        else
            gameObject.layer = LayerMask.NameToLayer("InteractiveWorld");

        // It only subscribes to events if it is going to change worlds
        if (belongsTo != World.BOTH) {
            GameEvents.instance.onNormalWorldEnter += NormalWorldEnter;
            GameEvents.instance.onArcaneWorldEnter += ArcaneWorldEnter;
        }
    }

    void NormalWorldEnter(Vector3 effectOrigin) {
        float timeUntilEffect = (WorldsController.instance.maxRadius - Vector3.Distance(transform.position, effectOrigin)) / WorldsController.instance.effectSpeed;
        StartCoroutine(HoldEffect(timeUntilEffect, true));
    }

    void ArcaneWorldEnter(Vector3 effectOrigin) {
        float timeUntilEffect = Vector3.Distance(transform.position, effectOrigin) / WorldsController.instance.effectSpeed;
        StartCoroutine(HoldEffect(timeUntilEffect, false));
    }

    IEnumerator HoldEffect(float timeToWait, bool isEnteringNormal) {
        yield return new WaitForSeconds(timeToWait);
        if (belongsTo == World.NORMAL) {
            if (isEnteringNormal) {
                // Will appear
                gameObject.layer = LayerMask.NameToLayer("InteractiveWorld");
            }
            else {
                // Will disappear (can change worlds)
                gameObject.layer = LayerMask.NameToLayer("UninteractiveWorld");
                CheckForObjectWorldChange(true);
            }
        }
        else if (belongsTo == World.ARCANE) {
            if (!isEnteringNormal) {
                // Will appear
                gameObject.layer = LayerMask.NameToLayer("InteractiveWorld");
            }
            else {
                // Will disappear (can change worlds)
                gameObject.layer = LayerMask.NameToLayer("UninteractiveWorld");
                CheckForObjectWorldChange(false);
            }
        }
        // If it belongs to both then the shader does the work
    }

    void CheckForObjectWorldChange(bool belongsToNormal) {
        // Will change worlds if it is a pickable object and the world changer is interacting with it
        if (pickableObject != null && pickableObject.isInteracting) {
            Texture text = GetComponent<Renderer>().material.mainTexture;

            // If it belongs to the Normal world then it will change to the Arcane world, and vice-versa
            if (belongsToNormal) {
                belongsTo = World.ARCANE;
                GetComponent<Renderer>().material = new Material(Shader.Find("Custom/AppearShader"));
            }
            else {
                belongsTo = World.NORMAL;
                GetComponent<Renderer>().material = new Material(Shader.Find("Custom/DisappearShader"));
            }

            GetComponent<Renderer>().material.SetTexture("_MainTex", text);
            gameObject.layer = LayerMask.NameToLayer("InteractiveWorld");
        }
    }

    void OnDestroy() {
        // Make sure to always unsubscribe the events when the object no longer exists
        if (belongsTo != World.BOTH) {
            GameEvents.instance.onNormalWorldEnter -= NormalWorldEnter;
            GameEvents.instance.onArcaneWorldEnter -= ArcaneWorldEnter;
        }
    }

}
