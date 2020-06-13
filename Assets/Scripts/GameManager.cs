using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public bool StartTimer { get; private set; }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            StartTimer = true;
            StartCoroutine(HoldTimer());
        }

        if (Input.GetKeyUp(KeyCode.F1)) {
            StartTimer = false;
        }
    }

    public void LoadScene(string sceneToLoad) {
        //PlayerPrefs.Save();
        //Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        Debug.Log("Quit Game");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //public void SetCursorVisibility(bool isVisible) {
    //    if (isVisible) {
    //        Cursor.visible = true;
    //        Cursor.lockState = CursorLockMode.None;
    //    }
    //    else {
    //        Cursor.visible = false;
    //        Cursor.lockState = CursorLockMode.Locked;
    //    }
    //}

    IEnumerator HoldTimer() {
        Debug.Log("Starting Timer!");
        yield return new WaitForSeconds(3f);
        if (StartTimer)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void createHint(string title, string text) {
        // Remove if a Hint already exists
        if (GameObject.FindGameObjectWithTag("UI").transform.GetChild(1).CompareTag("Hint")) {
            GameObject currentHint = GameObject.FindGameObjectWithTag("UI").transform.GetChild(1).gameObject;
            LeanTween.cancel(currentHint.gameObject);
            currentHint.GetComponent<UITweener>().SwapDirection();
            currentHint.GetComponent<UITweener>().DisableAfterDelay(0f);
        }

        GameObject hint = Instantiate(Resources.Load("Hint") as GameObject, GameObject.FindGameObjectWithTag("UI").transform);
        hint.transform.SetSiblingIndex(1);
        hint.GetComponent<HintScript>().SetHintTitle(title);
        hint.GetComponent<HintScript>().SetHintText(text);
    }

}
