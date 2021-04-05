using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    Button nextLevelButton;
    public string nextLevelSceneName;

    void Start()
    {
        nextLevelButton = GetComponent<Button>();
        nextLevelButton.onClick.AddListener(LoadNextLevel);
    }

    void LoadNextLevel(){
        Debug.Log("loading scene: " + nextLevelSceneName);
        SceneManager.LoadScene(nextLevelSceneName);
    }
}
