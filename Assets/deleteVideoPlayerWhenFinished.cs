using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class deleteVideoPlayerWhenFinished : MonoBehaviour
{
    private bool started_playing = false;
    public VideoPlayer myVidPlayer;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(myVidPlayer.isPlaying) {
            started_playing = true;
        }
        if(started_playing && !myVidPlayer.isPlaying) {
            canvas.SetActive(true);
            Destroy(gameObject);
        }
    }
}
