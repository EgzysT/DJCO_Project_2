using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class Collectable : InteractableObject
{
    public override void Interacting() { }

    public override void LeftMouseButtonDown()
    {
        Destroy(gameObject);
    }

    public override void LeftMouseButtonUp() { }

    public override void PressR() { }

    public override void RightMouseButtonDown() { }

    void OnDestroy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        RuntimeManager.PlayOneShotAttached(GetComponent<StudioEventEmitter>().Event, player);
    }
}
