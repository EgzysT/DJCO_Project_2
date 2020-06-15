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

    [Header("Checkpoint Settings")]
    public bool saveCheckpoint;

    void Start() {
        gameObject.layer = LayerMask.NameToLayer("Trigger");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (hintTitle != "" && hintText != "")
                GameManager.createHint(hintTitle, hintText);

            if (subtitleText != "")
                GameManager.createSubtitle(subtitleText);

            if (saveCheckpoint) {
                PlayerPrefs.SetFloat("player_position.x", other.transform.position.x);
                PlayerPrefs.SetFloat("player_position.y", other.transform.position.y);
                PlayerPrefs.SetFloat("player_position.z", other.transform.position.z);
            }

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
