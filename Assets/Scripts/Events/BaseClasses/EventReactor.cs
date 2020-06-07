using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventReactor : MonoBehaviour
{
    [Header("Event react ID (negative to not react)")]
    public int id = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (id >= 0) {
            GameEvents.instance.onInteractableActivate += Activate;
            GameEvents.instance.onInteractableDeactivate += Deactivate;
        }
        else {
            Debug.LogWarning("[" + gameObject.name + "] - EventReactor without valid id");
        }

        StartEvent();
    }

    protected virtual void StartEvent() { }

    private void Activate(int id) {
        if (id == this.id) {
            Activate();
        }
    }

    public abstract void Activate();

    private void Deactivate(int id) {
        if (id == this.id) {
            Deactivate();
        }
    }

    public abstract void Deactivate();

    void OnDestroy() {
        if (id >= 0) {
            GameEvents.instance.onInteractableActivate -= Activate;
            GameEvents.instance.onInteractableDeactivate -= Deactivate;
        }
    }

}
