using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject volumeMenu;

    public bool pauseMenuAvailable;
    private GameObject player;
    private AudioSettings audioSettings;

    // Start is called before the first frame update
    void Start() {
        //pauseMenuAvailable = true;
        player = GameObject.FindGameObjectWithTag("Player");
        audioSettings = volumeMenu.GetComponent<AudioSettings>();
        audioSettings.InitializeVolumeSettings();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuAvailable) {
            if (!menu.activeSelf)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame() {
        StartCoroutine(PauseMenuTimer(0.4f));

        Time.timeScale = 0f;
        SetPlayerInteraction(false);
        SetCursorVisibility(true);
        menu.SetActive(true);
        audioSettings.SetAudioPause(true);
    }

    public void ResumeGame() {
        StartCoroutine(PauseMenuTimer(0.4f));

        Time.timeScale = 1f;
        SetPlayerInteraction(true);
        SetCursorVisibility(false);
        menu.GetComponent<UITweener>().Disable();
        audioSettings.SetAudioPause(false);
    }

//    public void QuitGame() {
//#if UNITY_EDITOR
//        Debug.Log("Quit Game");
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//        Application.Quit();
//#endif
//    }

    //public void LoadScene(string sceneToLoad) {
    //    SceneManager.LoadScene(sceneToLoad);
    //}

    public void SetCursorVisibility(bool isVisible) {
        if (isVisible) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void SetPlayerInteraction(bool canInteract) {
        player.GetComponent<Interactor>().setInteract(canInteract);
    }

    IEnumerator PauseMenuTimer(float duration) {
        pauseMenuAvailable = false;
        yield return new WaitForSecondsRealtime(duration);
        pauseMenuAvailable = true;
    }
}
