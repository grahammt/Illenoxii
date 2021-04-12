using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MusicStates {
    menu,
    game,
};

public class MainAudioController : MonoBehaviour
{
    public AudioSource myAudioSource;
    public AudioClip menu_song;
    public AudioClip gameplay_song;
    static string [] tut_scene_names = {"tutorial1", "tutorial2", "tutorial3", "tutorial4", "Tutorial5"};
    static string [] gameplay_scene_names = {"level 1", "level 2", "level 3", "level 4", "FlameEnemyLevel", "Boss 1"};
    private MusicStates current_music;
    private Subscription<IntroVideoOverEvent> mySub;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        mySub = EventBus.Subscribe<IntroVideoOverEvent>(_IntroVideoOver);
        SceneManager.activeSceneChanged += _SceneCheck;
        current_music = MusicStates.menu;
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _IntroVideoOver(IntroVideoOverEvent e) {
        Debug.Log("MUSIC: Initial video over");
        current_music = MusicStates.menu;
        myAudioSource.Play();
    }
    void _SceneCheck(Scene scene, Scene next) {
        MusicStates old_state = current_music;
        bool found = false;
        foreach (string sn in tut_scene_names) {
            if(next.name == sn) {
                Debug.Log("MUSIC: next scene found");
                found = true;
                current_music = MusicStates.menu;
                myAudioSource.clip = menu_song;
                if(current_music != old_state) {
                    Debug.Log("MUSIC: Playing new song");
                    myAudioSource.Play();
                    myAudioSource.volume = 0.2f;
                }
            }
        }
        foreach (string sn in gameplay_scene_names) {
            if(next.name == sn) {
                Debug.Log("MUSIC: next scene found");
                found = true;
                current_music = MusicStates.game;
                myAudioSource.clip = gameplay_song;
                if(current_music != old_state) {
                    Debug.Log("MUSIC: Playing new song");
                    myAudioSource.Play();
                    myAudioSource.volume = 0.4f;
                }
            }
        }
        if(!found) {
            Debug.Log("MUSIC: Destorying music object");
            gameObject.SetActive(false);
            EventBus.Unsubscribe<IntroVideoOverEvent>(mySub);
            SceneManager.activeSceneChanged += _SceneCheck;
        }
    }
}
