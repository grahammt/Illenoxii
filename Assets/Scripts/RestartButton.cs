using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    Button restartButton;

    void Start()
    {
        restartButton = GetComponent<Button>();
        restartButton.onClick.AddListener(RestartLevel);
    }

    void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PausedGameManager.is_paused = false;
        Time.timeScale = 1;
    }
}
