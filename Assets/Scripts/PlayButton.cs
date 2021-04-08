using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    Button playButton;

    void Start()
    {
        playButton = GetComponent<Button>();
        playButton.onClick.AddListener(StartGame);
    }

    void StartGame(){
        SceneManager.LoadScene(1);
    }
}
