﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnImpact : MonoBehaviour
{
    public GameObject sender;
    public IEnumerator timer = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject != sender){
            if (other.gameObject.tag == "Enemy"){
                other.gameObject.GetComponent<HasHealth>().takeDamage(25);
                //player.GetComponent<InputToPlayerAttacks>().SecondaryHit();
            }
            if (other.gameObject.tag != "Player"){
                Destroy(gameObject);
            }
        }
    }
}
