using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerSoundController playerSoundController;
    private CameraShake cameraShake;
    private bool readyToPlaySound;
    private float timer = 0;
    public float secondsUntilDeath = 3;

    private enum AFRAID_STATE { AFRAID, NOT_AFRAID };
    private AFRAID_STATE currentAfraidState;
    private bool ded;

    private void Start()
    {
        cameraShake = GetComponentInChildren<CameraShake>();
        playerSoundController = GetComponentInChildren<PlayerSoundController>();
        readyToPlaySound = true;
        currentAfraidState = AFRAID_STATE.NOT_AFRAID;
    }

    public void StartAfraid()
    {
        currentAfraidState = AFRAID_STATE.AFRAID;
        cameraShake.DoAfraidEffect();
            playerSoundController.PlayAfraidSound();
        //StartCoroutine(WaitToPlaySoundAgain());
    }

    public void StopAfraid()
    {
        cameraShake.StopAfraidEffect();
        currentAfraidState = AFRAID_STATE.NOT_AFRAID;
    }

    private void Update()
    {
        if (ded)
        {
            Debug.Log("ded");
            return;
        }

        if (currentAfraidState == AFRAID_STATE.AFRAID)
        {
            Debug.Log(timer);
            if (timer > secondsUntilDeath)
            {
                timer = 0.0f;
                Die();
            }
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0.0f;   
        }   
    }

    private void Die()
    {
        ded = true;
    }

/*    IEnumerator WaitToPlaySoundAgain()
    {
        readyToPlaySound = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(5, 15));
        readyToPlaySound = true;
    }*/
}
