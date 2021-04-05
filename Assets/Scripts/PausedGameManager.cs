using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedGameManager : MonoBehaviour
{
    public static bool is_paused = false;
    public GameObject pauseMenu;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")) {
            is_paused = !is_paused;
            Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
            pauseMenu.SetActive(is_paused);
        }
    }
}
