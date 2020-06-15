using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private PlayerSoundController playerSoundController;
    private GameManager gameManager;
    private CameraShake cameraShake;
    private float timer = 0;
    public float secondsUntilDeath = 3;
    private bool died;

    private enum AFRAID_STATE { AFRAID, NOT_AFRAID };
    private AFRAID_STATE currentAfraidState;

    private void Start()
    {
        float newPositionX = PlayerPrefs.GetFloat("player_position.x", float.NaN);
        float newPositionY = PlayerPrefs.GetFloat("player_position.y", float.NaN);
        float newPositionZ = PlayerPrefs.GetFloat("player_position.z", float.NaN);
        if (!float.IsNaN(newPositionX) && !float.IsNaN(newPositionY) && !float.IsNaN(newPositionZ))
        {
            transform.position = new Vector3(newPositionX, newPositionY, newPositionZ);
        }

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraShake = GetComponentInChildren<CameraShake>();
        playerSoundController = GetComponentInChildren<PlayerSoundController>();
        currentAfraidState = AFRAID_STATE.NOT_AFRAID;
    }

    public void StartAfraid()
    {
        currentAfraidState = AFRAID_STATE.AFRAID;
        cameraShake.DoAfraidEffect();
        playerSoundController.PlayAfraidSound();
    }

    public void StopAfraid()
    {
        cameraShake.StopAfraidEffect();
        currentAfraidState = AFRAID_STATE.NOT_AFRAID;
    }

    private void Update()
    {
        if (died) return;

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
        died = true;
        gameManager.ReloadScene();
    }

    private void OnDestroy()
    {
        if (!died)
        {
            PlayerPrefs.DeleteKey("player_position.x");
            PlayerPrefs.DeleteKey("player_position.y");
            PlayerPrefs.DeleteKey("player_position.z");
        }
    }
}
