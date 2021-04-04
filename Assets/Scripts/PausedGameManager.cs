using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedGameManager : MonoBehaviour
{
    public static bool is_paused = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            is_paused = !is_paused;
            Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        }
    }
}
