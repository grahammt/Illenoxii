using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndVideoController : MonoBehaviour
{
    private bool started_playing = false;
    public VideoPlayer myVid;

    // Update is called once per frame
    void Update()
    {
        if(myVid.isPlaying)  {
            started_playing = true;
        }
        if(started_playing && !myVid.isPlaying) {
            SceneManager.LoadScene(0);
        }
    }
}
