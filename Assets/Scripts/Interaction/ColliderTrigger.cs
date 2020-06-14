using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : EventTrigger {

    public enum Trigger { Activate, Deactivate };

    public Trigger triggerAction;
    public bool multipleInteractions;
    public bool shouldDestroyGameObject;

    void Start() {
        gameObject.layer = LayerMask.NameToLayer("Trigger");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (triggerAction == Trigger.Activate)
                TriggerActivate();
            else
                TriggerDeactivate();

            if (!multipleInteractions)
                if (shouldDestroyGameObject)
                    Destroy(gameObject);
                else
                    Destroy(this);
        }
    }

}
