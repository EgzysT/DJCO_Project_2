using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public bool StartTimer { get; private set; }
    public GameObject pauseMenu;

    private GameObject player;

    // Start is called before the first frame update
    void Start() {
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

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pauseMenu.activeSelf)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        SetPlayerInteraction(false);
        SetCursorVisibility(true);
        pauseMenu.SetActive(true);
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        SetPlayerInteraction(true);
        SetCursorVisibility(false);
        pauseMenu.GetComponent<UITweener>().Disable();
        //pauseMenu.SetActive(false);
    }

    public void QuitGame() {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private void SetCursorVisibility(bool isVisible) {
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

    IEnumerator HoldTimer() {
        Debug.Log("Starting Timer!");
        yield return new WaitForSeconds(3f);
        if (StartTimer)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
