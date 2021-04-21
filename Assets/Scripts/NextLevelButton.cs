using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    Button nextLevelButton;

    void Start()
    {
        nextLevelButton = GetComponent<Button>();
        nextLevelButton.onClick.AddListener(LoadNextLevel);
        PausedGameManager.is_paused=true;
        PausedGameManager.won = true;
    }

    void LoadNextLevel(){
        Debug.Log("button clicked");
        StartCoroutine("LoadSceneAfterDelay", SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LoadSceneAfterDelay(int sceneIndex){
        ComponentBank.instance.SceneTransitionAnimator.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        PausedGameManager.is_paused=false;
        SceneManager.LoadScene(sceneIndex);
    }
}
