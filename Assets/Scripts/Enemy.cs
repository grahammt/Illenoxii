using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void DeathCallBack();
    public DeathCallBack deathCallBack;

    HasHealth healthScript;
    CanGetKnockedBack knockbackScript;
    HasArmor armorScript;

    SpriteRenderer sprite;

    public DamageText damageTextPrefab;
    public AudioClip [] hitSounds;
    public AudioClip deathSound;

    private bool spawned = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        healthScript = GetComponent<HasHealth>();
        knockbackScript = GetComponent<CanGetKnockedBack>();
        armorScript = GetComponent<HasArmor>();
        deathCallBack += PostDieEvent;
    }

    void Update(){
        if (!spawned){
            EventBus.Publish<EnemySpawnEvent>(new EnemySpawnEvent());
            spawned = true;
        }
    }

    public void HandleHit(float dmg, float knockback){
        // take damage
        bool dead = healthScript.TakeDamage(dmg);

        // spawn damage text
        DamageText dmgtxt = GameObject.Instantiate(damageTextPrefab);
        dmgtxt.damage = dmg;
        dmgtxt.transform.position = transform.position;

        // add upward force to simulate knock up
        if(knockbackScript) knockbackScript.knockback(knockback);

        // add toward stun counter
        if(armorScript) armorScript.AddDamage(dmg);

        // add to combo counter
        EventBus.Publish<IncrementCombo>(new IncrementCombo());

        // play the hit sound
        int hit_index = Random.Range(0, hitSounds.Length);
        AudioSource.PlayClipAtPoint(hitSounds[hit_index], transform.position);

        // if dead
        if (dead)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            deathCallBack();
        }
    }

    void PostDieEvent(){
        EventBus.Publish<EnemyDieEvent>(new EnemyDieEvent());
    }

}
