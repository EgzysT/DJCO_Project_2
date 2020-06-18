using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class Collectable : InteractableObject {

    public bool giveChangeWorldsAbility;
    public string subtitleText = "";
    public override void Interacting() { }

    public override void LeftMouseButtonDown() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        RuntimeManager.PlayOneShotAttached(GetComponent<StudioEventEmitter>().Event, player);

        if (giveChangeWorldsAbility && !GameObject.FindGameObjectWithTag("Player").GetComponent<WorldsController>().canChangeWorlds) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<WorldsController>().canChangeWorlds = true;
            GameObject.FindGameObjectWithTag("CrystalIcon").GetComponent<UITweener>().enabled = true;
            GameManager.CreateHint("World Shift", "Press 'Q' to change worlds.\nYou can change the world an object belongs to, by holding it while changing worlds.");
        }

        if (subtitleText != "")
            GameManager.CreateSubtitle(subtitleText);

        TriggerActivate();
        Destroy(gameObject);
    }

    public override void LeftMouseButtonUp() { }

    public override void PressR() { }

    public override void RightMouseButtonDown() { }

}
