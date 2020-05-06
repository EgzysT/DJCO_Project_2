using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool isInteracting = false;
    public virtual void LeftMouseButtonDown() { }
    public virtual void LeftMouseButtonUp() { }
    public virtual void RightMouseButtonDown() { }
    public virtual void PressR() { }
    public virtual void Interacting() { }
}
