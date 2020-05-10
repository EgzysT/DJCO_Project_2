using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapWorld : MonoBehaviour
{
    void Update()
    {
        Shader.SetGlobalVector("_Position", transform.position);
    }
}
