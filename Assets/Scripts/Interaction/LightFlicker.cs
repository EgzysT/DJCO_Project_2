using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [MinMaxSlider(0.01f, 20.0f)]
    public Vector2 rangeWhileOn;
    
    [MinMaxSlider(0.01f, 20.0f)]
    public Vector2 rangeWhileOff;

    private bool isFlickering = false;
    private float timeDelay;

    // Update is called once per frame
    void Update()
    {
        if (isFlickering == false) {
            StartCoroutine(FlickeringLight());
        }   
    }

    IEnumerator FlickeringLight() {
        isFlickering = true;

        gameObject.GetComponent<Light>().enabled = false;
        timeDelay = Random.Range(rangeWhileOff.x, rangeWhileOff.y);

        yield return new WaitForSeconds(timeDelay);

        gameObject.GetComponent<Light>().enabled = true;
        timeDelay = Random.Range(rangeWhileOn.x, rangeWhileOn.y);

        yield return new WaitForSeconds(timeDelay);

        isFlickering = false;
    }

}
