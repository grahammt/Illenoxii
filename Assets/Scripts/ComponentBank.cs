using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBank : MonoBehaviour
{
    #region Singleton
    public static ComponentBank instance;
    void Awake(){
        if (instance != null){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }
    #endregion

    public Animator SceneTransitionAnimator;
}
