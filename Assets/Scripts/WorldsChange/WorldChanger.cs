using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shader))]
public class WorldChanger : MonoBehaviour
{
    public World belongsTo;

    // Start is called before the first frame update
    void Start()
    {
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
        float timeUntilEffect = (50f - Vector3.Distance(transform.position, effectOrigin)) / 15f;
        StartCoroutine(HoldEffect(timeUntilEffect, true));
    }

    void ArcaneWorldEnter(Vector3 effectOrigin) {
        float timeUntilEffect = Vector3.Distance(transform.position, effectOrigin) / 15f;
        StartCoroutine(HoldEffect(timeUntilEffect, false));
    }

    IEnumerator HoldEffect(float timeToWait, bool isEnteringNormal) {
        yield return new WaitForSeconds(timeToWait);
        if (belongsTo == World.NORMAL)
            gameObject.layer = isEnteringNormal ? LayerMask.NameToLayer("InteractiveWorld") : LayerMask.NameToLayer("UninteractiveWorld");
        else if (belongsTo == World.ARCANE)
            gameObject.layer = !isEnteringNormal ? LayerMask.NameToLayer("InteractiveWorld") : LayerMask.NameToLayer("UninteractiveWorld");
        // If it belongs to both then the shader does the work
    }

    void OnDestroy() {
        // Make sure to always unsubscribe the events when the object no longer exists
        if (belongsTo != World.BOTH) {
            GameEvents.instance.onNormalWorldEnter -= NormalWorldEnter;
            GameEvents.instance.onArcaneWorldEnter -= ArcaneWorldEnter;
        }
    }

}
