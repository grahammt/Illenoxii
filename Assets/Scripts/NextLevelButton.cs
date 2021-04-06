using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    Button nextLevelButton;
    public int nextLevelSceneIndex;

    void Start()
    {
        nextLevelButton = GetComponent<Button>();
        nextLevelButton.onClick.AddListener(LoadNextLevel);
    }

    void LoadNextLevel(){
        Debug.Log("button clicked");
        StartCoroutine("LoadSceneAfterDelay", nextLevelSceneIndex);
    }

    IEnumerator LoadSceneAfterDelay(int sceneIndex){
        ComponentBank.instance.SceneTransitionAnimator.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
    }
}
