using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainAudioController : MonoBehaviour
{
    public AudioSource myAudioSource;
    static string [] scene_names = {"tutorial1", "tutorial2", "tutorial3"};
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        EventBus.Subscribe<IntroVideoOverEvent>(_IntroVideoOver);
        SceneManager.activeSceneChanged += _SceneCheck;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _IntroVideoOver(IntroVideoOverEvent e) {
        myAudioSource.Play();
    }
    void _SceneCheck(Scene scene, Scene next) {
        Debug.Log("SCENE NAME: " + next.name);
        foreach (string sn in scene_names) {
            if(next.name == sn) {
                Destroy(this);
            }
        }
    }
}
