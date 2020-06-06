using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventTrigger : MonoBehaviour
{
    [Header("Event trigger ID (negative to not trigger)")]
    public int id = -1;

    protected void TriggerActivate() {
        if (id >= 0) {
            GameEvents.instance.InteractableActivate(id);
        }
        else {
            Debug.LogWarning("[" + gameObject.name + "] - Calling TriggerActivate without specifying valid id");
        }
    }

    protected void TriggerDeactivate() {
        if (id >= 0) {
            GameEvents.instance.InteractableDeactivate(id);
        }
        else {
            Debug.LogWarning("[" + gameObject.name + "] - Calling TriggerDeactivate without specifying valid id");
        }
    }
}
