﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public float maxHealth;
    public HealthBar healthBar;
    float currentHealth;
    Rigidbody2D rigidbody;
    Animator animator;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        if(healthBar)
            healthBar.SetMaxHealth(maxHealth);
    }

    public void takeDamage(float dmg){
        currentHealth -= dmg;
        if(healthBar)
            healthBar.SetCurrHealth(currentHealth);
        if (gameObject.CompareTag("Enemy"))
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, 0f);
            rigidbody.AddForce(new Vector3(0,200,0));
            if(animator)
                animator.SetTrigger("Dazed");
        }
        Debug.Log("Took " + dmg + " dmg");
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        if (gameObject.tag == "Player"){
            EventBus.Publish<ResetComboEvent>(new ResetComboEvent(0));
        }
        else if (gameObject.tag == "Enemy"){
            EventBus.Publish<IncrementCombo>(new IncrementCombo());
        }
    }

    public void takeDamage(float dmg, float knockback)
    {
        currentHealth -= dmg;
        if(healthBar)
            healthBar.SetCurrHealth(currentHealth);
        if (gameObject.CompareTag("Enemy"))
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, 0f);
            rigidbody.AddForce(new Vector3(0,knockback,0));
            if(animator)
                animator.SetTrigger("Dazed");
        }
        Debug.Log("Took " + dmg + " dmg");
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        if (gameObject.tag == "Player")
        {
            EventBus.Publish<ResetComboEvent>(new ResetComboEvent(0));
        }
        else if (gameObject.tag == "Enemy")
        {
            EventBus.Publish<IncrementCombo>(new IncrementCombo());
        }
    }
}

public class ResetComboEvent {
    public int new_combo = 0;
    public ResetComboEvent(int _new_combo){
        new_combo = _new_combo;
    }

    public override string ToString(){
        return "Combo x" + new_combo + "!";
    }
}

public class IncrementCombo{
    public int inc_amt = 1;
    public IncrementCombo(){}
    public IncrementCombo(int _inc_amt){
        inc_amt = _inc_amt;
    }
}
