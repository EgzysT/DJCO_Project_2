using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : EventTrigger {

    public enum Trigger { Activate, Deactivate };

    public Trigger triggerAction;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (triggerAction == Trigger.Activate)
                TriggerActivate();
            else
                TriggerDeactivate();
        }
    }

}
