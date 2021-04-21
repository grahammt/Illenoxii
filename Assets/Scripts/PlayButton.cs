using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    Button playButton;
    public GameObject fader;

    void Start()
    {
        playButton = GetComponent<Button>();
        playButton.onClick.AddListener(StartGame);
    }

    void StartGame(){
        StartCoroutine("LoadSceneAfterDelay", SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LoadSceneAfterDelay(int sceneIndex){
        fader.SetActive(true);
        fader.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
    }
}
