using System.Collections;
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
        PlayerPrefs.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void RemoveLastCheckpoint() {
        PlayerPrefs.DeleteKey("player_position.x");
        PlayerPrefs.DeleteKey("player_position.y");
        PlayerPrefs.DeleteKey("player_position.z");
        PlayerPrefs.DeleteKey("player_changeWorlds");
        PlayerPrefs.Save();
    }

    public void ReloadScene() {
        PlayerPrefs.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    public static void ClearHintsAndSubtitles() {
        Destroy(GameObject.FindGameObjectWithTag("HintHolder"));
        Destroy(GameObject.FindGameObjectWithTag("SubtitleHolder"));
    }

    public static void CreateHint(string title, string text) {
        // Remove if a Hint already exists
        Transform tmp = GameObject.FindGameObjectWithTag("HintHolder").transform.Find("Hint(Clone)");
        if (tmp != null) {
            LeanTween.cancel(tmp.gameObject);
            tmp.gameObject.GetComponent<UITweener>().SwapDirection();
            tmp.gameObject.GetComponent<UITweener>().DisableAfterDelay(0f);
        }

        GameObject hint = Instantiate(Resources.Load("Hint") as GameObject, GameObject.FindGameObjectWithTag("HintHolder").transform);
        hint.GetComponent<HintScript>().SetHintTitle(title);
        hint.GetComponent<HintScript>().SetHintText(text);
    }

    public static void CreateSubtitle(string text) {
        //Remove if a Subtitle already exists
        GameObject subtitleHolder = GameObject.FindGameObjectWithTag("SubtitleHolder");
        for (int i = 0; i < subtitleHolder.transform.childCount; i++) {
            subtitleHolder.transform.GetChild(i).GetComponent<UITweener>().MoveSubtitleUp(40f, 0.3f);
        }

        GameObject subtitle = Instantiate(Resources.Load("Subtitle") as GameObject, subtitleHolder.transform);
        subtitle.GetComponent<HintScript>().SetHintText(text);
    }

    public void SubtitleNoStatic(string text)
    {
        CreateSubtitle(text);
    }

}
