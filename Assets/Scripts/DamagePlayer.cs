using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public float damage;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            other.gameObject.SendMessage("takeDamage", damage);
            /*GameObject player = GameObject.Find("Player");
            if (player!= null){
                player.GetComponent<InputToPlayerAttacks>().SecondaryHit();
            }*/
        }
    }
}
