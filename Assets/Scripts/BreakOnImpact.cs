﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnImpact : MonoBehaviour
{
    public GameObject sender;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject != sender && sender.CompareTag("Player")){
            if (other.gameObject.GetComponent<Enemy>()!=null){
                other.gameObject.GetComponent<Enemy>().HandleHit(damage, 0);
                //player.GetComponent<InputToPlayerAttacks>().SecondaryHit();
                Destroy(gameObject);
            }
            
            
        }
        if (other.gameObject != sender && sender.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<HasHealth>() != null && other.gameObject.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<PlayerDriver>().parrying)
                {
                    sender = gameObject;
                    gameObject.GetComponent<Rigidbody2D>().velocity = -gameObject.GetComponent<Rigidbody2D>().velocity;
                }
                else
                {
                    other.gameObject.GetComponent<PlayerDriver>().HandleHit(damage);
                    Destroy(gameObject);
                }
                
                //player.GetComponent<InputToPlayerAttacks>().SecondaryHit();
            }


        }
        
    }
}
