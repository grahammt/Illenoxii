using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedByParry : MonoBehaviour
{
    public delegate void StunCallback();
    public StunCallback stunCallback;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            PlayerDriver tmp = other.GetComponent<PlayerDriver>();
            if(tmp.parrying){
                // stun
                stunCallback();
            }
        }
    }
}
