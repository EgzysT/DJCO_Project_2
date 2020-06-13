using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerSoundController playerSoundController;
    private CameraShake cameraShake;
    private bool readyToBeAfraid;

    private void Start()
    {
        cameraShake = GetComponentInChildren<CameraShake>();
        playerSoundController = GetComponentInChildren<PlayerSoundController>();
        readyToBeAfraid = true;
    }

    public void StartAfraid()
    {
        if (!readyToBeAfraid) return;

        cameraShake.DoAfraidEffect();
        playerSoundController.PlayAfraidSound();
        StartCoroutine(WaitToBeAfraid());
    }

    IEnumerator WaitToBeAfraid()
    {
        readyToBeAfraid = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(10,20));
        readyToBeAfraid = true;
    }

    public void StopAfraid()
    {
        /*if(UnityEngine.Random.Range(0,100) <= 25)
            playerSoundController.StopAfraidSound();*/
        cameraShake.StopAfraidEffect();
    }
}
