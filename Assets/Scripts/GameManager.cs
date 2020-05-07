using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool StartTimer { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartTimer = true;
            StartCoroutine(HoldTimer());
        }

        if (Input.GetKeyUp(KeyCode.F1))
        {
            StartTimer = false;
        }
    }

    IEnumerator HoldTimer()
    {
        Debug.Log("Starting Timer!");
        yield return new WaitForSeconds(3f);
        if (StartTimer)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
