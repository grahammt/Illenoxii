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
        if (gameObject.CompareTag("Enemy"))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0,300,0));
        }
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        Debug.Log("Took " + dmg + " dmg");
    }
}
