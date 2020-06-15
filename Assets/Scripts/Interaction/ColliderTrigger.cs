using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : EventTrigger {

    public enum Trigger { Activate, Deactivate };

    [Header("Trigger Settings")]
    public Trigger triggerAction;
    public bool multipleInteractions;
    public bool shouldDestroyGameObject;

    [Header("Hint Settings")]
    public string hintTitle = "";
    public string hintText = "";

    [Header("Subtitle Settings")]
    public string subtitleText = "";

    void Start() {
        gameObject.layer = LayerMask.NameToLayer("Trigger");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (hintTitle != "" && hintText != "")
                GameManager.createHint(hintTitle, hintText);

            if (subtitleText != "")
                GameManager.createSubtitle(subtitleText);

            if (id >= 0) {
                if (triggerAction == Trigger.Activate)
                    TriggerActivate();
                else
                    TriggerDeactivate();
            }

            if (!multipleInteractions)
                if (shouldDestroyGameObject)
                    Destroy(gameObject);
                else
                    Destroy(this);
        }
    }

}
