using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duration;
    public float magnitude;
    public float afraidDuration;
    public float afraidMagnitude;
    private Coroutine afraidEffectCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.instance.onNormalWorldEnter += WorldChange;
        GameEvents.instance.onArcaneWorldEnter += WorldChange;
    }

    void WorldChange(Vector3 effectOrigin) {
        StartCoroutine(Shake(duration, magnitude));
    }

    public void DoAfraidEffect()
    {
        if (afraidEffectCoroutine == null)
        {
            afraidEffectCoroutine = StartCoroutine(AfraidEffect());
        }
    }

    public void StopAfraidEffect()
    {
        if (afraidEffectCoroutine != null)
        {
            StopCoroutine(afraidEffectCoroutine);
            afraidEffectCoroutine = null;
        }
    }

    IEnumerator Shake(float duration, float magnitude) {
        Vector3 originalPos = transform.localPosition;
        
        float elapsed = 0f;
        while(elapsed < duration) {
            transform.localPosition = originalPos + new Vector3(Random.Range(-1f, 1f) * magnitude, Random.Range(-1f, 1f) * magnitude, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    IEnumerator AfraidEffect()
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;
        while (elapsed < afraidDuration)
        {
            transform.localPosition = originalPos + new Vector3(Random.Range(-1f, 1f) * afraidMagnitude, Random.Range(-1f, 1f) * afraidMagnitude, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    private void OnDestroy() {
        // Make sure to always unsubscribe the events when the object no longer exists
        GameEvents.instance.onNormalWorldEnter -= WorldChange;
        GameEvents.instance.onArcaneWorldEnter -= WorldChange;
    }
}
