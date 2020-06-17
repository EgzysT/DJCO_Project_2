using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class Collectable : InteractableObject
{
    public string subtitleText = "";
    public override void Interacting() { }

    public override void LeftMouseButtonDown()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        RuntimeManager.PlayOneShotAttached(GetComponent<StudioEventEmitter>().Event, player);

        if (subtitleText != "")
            GameManager.CreateSubtitle(subtitleText);

        TriggerActivate();
        Destroy(gameObject);
    }

    public override void LeftMouseButtonUp() { }

    public override void PressR() { }

    public override void RightMouseButtonDown() { }

}
