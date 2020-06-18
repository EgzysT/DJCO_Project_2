using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    void Awake() {
        instance = this;
    }

    public event Action<Vector3> onNormalWorldEnter;
    public void NormalWorldEnter(Vector3 effectOrigin) {
        //Debug.Log("Changing to NORMAL");
        if (onNormalWorldEnter != null) {
            onNormalWorldEnter(effectOrigin);
        }
    }

    public event Action<Vector3> onArcaneWorldEnter;
    public void ArcaneWorldEnter(Vector3 effectOrigin) {
        //Debug.Log("Changing to ARCANE");
        if (onArcaneWorldEnter != null) {
            onArcaneWorldEnter(effectOrigin);
        }
    }

    public event Action<int> onInteractableActivate;
    public void InteractableActivate(int id) {
        if (onInteractableActivate != null) {
            onInteractableActivate(id);
        }
    }

    public event Action<int> onInteractableDeactivate;
    public void InteractableDeactivate(int id) {
        if (onInteractableDeactivate != null) {
            onInteractableDeactivate(id);
        }
    }


}
