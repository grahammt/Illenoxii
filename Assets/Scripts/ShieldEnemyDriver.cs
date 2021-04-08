using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyDriver : MonoBehaviour
{
    public float chargeCooldown;
    public float chargeOvershoot;

    Transform target;
    SpriteRenderer sprite;
    Animator animator;
    float chargeCooldownTimer;
    bool charging = false;
    float chargeDestX;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        target = ComponentBank.instance.playerTransform;
        chargeCooldownTimer = chargeCooldown;
    }

    void Update()
    {
        // count down the timer while not charging
        if(!charging){
            sprite.flipX = target.position.x < transform.position.x;
            if(chargeCooldownTimer > 0){
                chargeCooldownTimer -= Time.deltaTime;
            }
            else{
                charging = true;
                chargeDestX = target.position.x + (sprite.flipX ? -1 : 1) * chargeOvershoot;
                animator.SetTrigger("Charge");
            }
        }
        // while charging, check if we've gone past our destination
        else{
            if(sprite.flipX && transform.position.x < chargeDestX){
                animator.SetTrigger("Stop");
                charging = false;
                chargeCooldownTimer = chargeCooldown;
            }
            else if(!sprite.flipX && transform.position.x > chargeDestX){
                animator.SetTrigger("Stop");
                charging = false;
                chargeCooldownTimer = chargeCooldown;
            }
        }
    }

    public void ResetChargeCD(){
        chargeCooldownTimer = chargeCooldown;
    }
}
