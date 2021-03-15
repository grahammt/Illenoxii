using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    public float damage;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            other.gameObject.SendMessage("takeDamage", damage);
            GameObject player = GameObject.Find("Player");
            if (player!= null){
                player.GetComponent<ComboTracker>().count++;
                player.GetComponent<ComboTracker>().SetComboText();
                player.GetComponent<InputToPlayerAttacks>().SecondaryHit();
            }
        }
    }
}
