using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : MonoBehaviour
{
    public string SceneToLoad;

    public void LoadGameScene() {
        SceneManager.LoadScene(SceneToLoad);
    }
}
