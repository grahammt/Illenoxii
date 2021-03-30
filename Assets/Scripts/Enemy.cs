using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    HasHealth healthScript;
    CanGetKnockedBack knockbackScript;
    HasArmor armorScript;

    SpriteRenderer sprite;

    public DamageText damageTextPrefab;
    public AudioClip hitSound;
    public AudioClip deathSound;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        healthScript = GetComponent<HasHealth>();
        knockbackScript = GetComponent<CanGetKnockedBack>();
        armorScript = GetComponent<HasArmor>();
    }

    public void HandleHit(float dmg, float knockback){
        // take damage
        bool dead = healthScript.TakeDamage(dmg);

        // spawn damage text
        DamageText dmgtxt = GameObject.Instantiate(damageTextPrefab);
        dmgtxt.damage = dmg;
        dmgtxt.transform.position = transform.position;

        // add upward force to simulate knock up
        knockbackScript.knockback(knockback);

        // add toward stun counter
        if(armorScript) armorScript.AddDamage(dmg);

        // add to combo counter
        EventBus.Publish<IncrementCombo>(new IncrementCombo());

        // play the hit sound
        AudioSource.PlayClipAtPoint(hitSound, transform.position);

        // if dead
        if (dead)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Destroy(gameObject);
        }
    }

}
