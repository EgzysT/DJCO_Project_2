using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerHealth : MonoBehaviour
{
    private PlayerSoundController playerSoundController;
    private CameraShake cameraShake;
    private float timer = 0;
    public float secondsUntilDeath = 3;
    public float secondsTimeOut = 6;
    private bool died;

    private enum AFRAID_STATE { AFRAID, NOT_AFRAID };
    private AFRAID_STATE currentAfraidState;
    private bool afraidTimeOut;

    private void Start()
    {
        LoadLastCheckPoint();

        cameraShake = GetComponentInChildren<CameraShake>();
        playerSoundController = GetComponentInChildren<PlayerSoundController>();
        currentAfraidState = AFRAID_STATE.NOT_AFRAID;
    }

    private void LoadLastCheckPoint()
    {
        float newPositionX = PlayerPrefs.GetFloat("player_position.x", float.NaN);
        float newPositionY = PlayerPrefs.GetFloat("player_position.y", float.NaN);
        float newPositionZ = PlayerPrefs.GetFloat("player_position.z", float.NaN);

        if (!float.IsNaN(newPositionX) && !float.IsNaN(newPositionY) && !float.IsNaN(newPositionZ))
        {
            transform.position = new Vector3(newPositionX, newPositionY, newPositionZ);
            GameObject.Find("---Cutscene Stuff---")?.SetActive(false);
        }

        GameEvents.instance.onNormalWorldEnter += (_) => StopAfraid();
    }

    public void StartAfraid()
    {
        if (!afraidTimeOut)
        {
            playerSoundController.PlayAfraidSound();
            currentAfraidState = AFRAID_STATE.AFRAID;
        }
        cameraShake.DoAfraidEffect();
    }

    public void StopAfraid()
    {
        StartCoroutine(WaitToBeAfraid());
        cameraShake.StopAfraidEffect();
        playerSoundController.StopAfraidSound();
        currentAfraidState = AFRAID_STATE.NOT_AFRAID;
    }

    private void Update()
    {
        if (died)
        {
            StopAfraid();
            return;
        }

        if (currentAfraidState == AFRAID_STATE.AFRAID)
        {
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
        died = true;
        currentAfraidState = AFRAID_STATE.NOT_AFRAID;
        playerSoundController.StopAllSounds();
        GameObject.Find("DeathCutscene").GetComponent<PlayableDirector>().Play();
    }

    IEnumerator WaitToBeAfraid()
    {
        //wait x seconds until player can be scared again
        afraidTimeOut = true;
        yield return new WaitForSeconds(secondsTimeOut);
        afraidTimeOut = false;
    }

}
