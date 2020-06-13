using FMODUnity;
using System.Collections;
using System.Collections.Generic;
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
        GetComponent<StudioEventEmitter>().Play();
    }
}
