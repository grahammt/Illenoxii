using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResumeButton : MonoBehaviour
{
    Button resumeButton;
    public GameObject pauseMenu;

    void Start()
    {
        resumeButton = GetComponent<Button>();
        resumeButton.onClick.AddListener(Resume);
    }

    void Resume(){
        PausedGameManager.is_paused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(PausedGameManager.is_paused);
    }
}
