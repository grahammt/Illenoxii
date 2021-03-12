using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public float maxHealth;
    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void takeDamage(float dmg){
        currentHealth -= dmg;
        Debug.Log("Took " + dmg + " dmg");
    }
}
