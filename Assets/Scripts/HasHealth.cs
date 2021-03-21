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
        if (gameObject.tag == "Player"){
            EventBus.Publish<ResetComboEvent>(new ResetComboEvent(0));
        }
        else if (gameObject.tag == "Enemy"){
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
