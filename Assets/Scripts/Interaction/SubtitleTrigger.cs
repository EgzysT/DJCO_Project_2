using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SubtitleTrigger : MonoBehaviour
{
    public string subtitleText;

    void Start() {
        GetComponent<Collider>().isTrigger = true;
    }

    public void DisplaySubtitle() {
        GameManager.createSubtitle(subtitleText);
        Destroy(this);
    }

    void OnDestroy() {
        Destroy(GetComponent<Collider>());
    }
}
