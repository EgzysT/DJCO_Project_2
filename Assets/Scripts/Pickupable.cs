using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private Renderer rend;
    private Shader defaultShader;
    private Shader outlinedShader;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        defaultShader = rend.material.shader;
        outlinedShader = Shader.Find("Outlined/UltimateOutline");
    }

    public void TurnOnShader()
    {
        rend.material.shader = outlinedShader;
    }
    public void TurnOffShader()
    {
        rend.material.shader = defaultShader;
    }

    /*private void OnMouseEnter()
    {
        if (Vector3.Distance(Camera.main.transform.position, transform.position) < DISTANCE)
            rend.material.shader = outlinedShader;
    }
    private void OnMouseExit()
    {
        rend.material.shader = defaultShader;
    }*/
}
