using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(QuitToMainMenu);
    }

    void QuitToMainMenu(){
        PausedGameManager.is_paused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
