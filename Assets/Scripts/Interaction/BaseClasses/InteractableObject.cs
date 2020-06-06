using UnityEngine;

public abstract class InteractableObject : EventTrigger
{
    [HideInInspector]
    public bool isInteracting = false;

    public abstract void LeftMouseButtonDown();

    public abstract void LeftMouseButtonUp();

    public abstract void RightMouseButtonDown();

    public abstract void PressR();

    public abstract void Interacting();
}
