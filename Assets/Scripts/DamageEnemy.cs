using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    public float damage;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<Enemy>().HandleHit(damage, 0);
            /*GameObject player = GameObject.Find("Player");
            if (player!= null){
                player.GetComponent<InputToPlayerAttacks>().SecondaryHit();
            }*/
        }
    }
}
