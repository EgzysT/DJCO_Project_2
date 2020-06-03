using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public bool StartTimer { get; private set; }
    public GameObject pauseMenu;

    private bool pauseMenuAvailable;
    private GameObject player;

    // Start is called before the first frame update
    void Start() {
        pauseMenuAvailable = true;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            StartTimer = true;
            StartCoroutine(HoldTimer());
        }

        if (Input.GetKeyUp(KeyCode.F1)) {
            StartTimer = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuAvailable) {
            if (!pauseMenu.activeSelf)
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
        pauseMenu.SetActive(true);
    }

    public void ResumeGame() {
        StartCoroutine(PauseMenuTimer(0.4f));

        Time.timeScale = 1f;
        SetPlayerInteraction(true);
        SetCursorVisibility(false);
        pauseMenu.GetComponent<UITweener>().Disable();
    }

    public void QuitGame() {
#if UNITY_EDITOR
        Debug.Log("Quit Game");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

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

    IEnumerator HoldTimer() {
        Debug.Log("Starting Timer!");
        yield return new WaitForSeconds(3f);
        if (StartTimer)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
