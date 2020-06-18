using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalIcon : MonoBehaviour {

    public Color normalColor;
    public Color arcaneColor;

    //private Image icon;

    //// Start is called before the first frame update
    //void Start() {
    //    icon = GetComponent<Image>();
    //}

    public void ChangeWorlds(World futureWorld, float effectDuration) {
        if (futureWorld == World.NORMAL)
            LeanTween.color(GetComponent<RectTransform>(), normalColor, effectDuration).setEase(LeanTweenType.easeInOutCubic);
        else if (futureWorld == World.ARCANE)
            LeanTween.color(GetComponent<RectTransform>(), arcaneColor, effectDuration).setEase(LeanTweenType.easeInOutCubic);
    }
}
