using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Sprite crosshairTexture;
    public Sprite interactTexture;
    public Sprite clickTexture;

    public bool showCursor = true;
    private Image image;

    private void Start()
    {
        image = GameObject.Find("Crosshair").GetComponent<Image>();
    }

    public void ShowGrab()
    {
        image.enabled = true;
        image.sprite = interactTexture;
    }

    public void ShowClick()
    {
        image.enabled = true;
        image.sprite = clickTexture;
    }

    public void ShowNone()
    {
        image.enabled = false;
    }

    public void ShowNormal()
    {
        image.enabled = true;
        image.sprite = crosshairTexture;
    }
}