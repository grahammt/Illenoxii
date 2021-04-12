using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyDriver : MonoBehaviour
{
    public float chargeCooldown = 5f;
    public float chargeOvershoot = 5f;
    public float aggroRange = 6f;

    public enum State{
        searching,
        buildup,
        charging,
        cooling,
        stunned
    };

    Transform target;
    SpriteRenderer sprite;
    Animator animator;
    Enemy enemyScript;
    State state = State.searching;
    public GameObject hitbox;
    public GameObject healthbar;

    float chargeCooldownTimer;
    float chargeDestX;

    StunnedByParry stunScript;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        stunScript = GetComponentInChildren<StunnedByParry>();
        enemyScript = GetComponentInChildren<Enemy>();
        target = ComponentBank.instance.playerTransform;

        stunScript.stunCallback += GetStunned;
        enemyScript.deathCallBack += DeathHandler;

        chargeCooldownTimer = chargeCooldown;
    }

    void Update()
    {
        switch(state){
        case State.searching:
            // walk and follow the player
            sprite.flipX = target.position.x < transform.position.x;

            // if player within aggro range, start transition into a charge
            if(Mathf.Abs(target.position.x - transform.position.x) < aggroRange){
                // default charge length is distance from play + charge overshoot
                float chargeLength = chargeOvershoot + Mathf.Abs(transform.position.x - target.position.x);

                // perform raycast to check for walls
                RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.down * 0.8f, (sprite.flipX) ? Vector2.left : Vector2.right, chargeLength, LayerMask.GetMask("Terrain"));
                if(hit.collider != null){
                    chargeLength = hit.distance - 1.5f;
                }

                // set charge destination and next state logic
                chargeDestX = transform.position.x + (sprite.flipX ? -1 : 1) * chargeLength;
                state = State.charging;
                animator.SetTrigger("TargetSet");
            }
            break;
        case State.buildup:
            // count down the timer while not charging
            /*if(chargeBuildupTimer > 0){
                chargeBuildupTimer -= Time.deltaTime;
            }
            else{
                state = State.charging;
                animator.SetTrigger("Charge");
            }*/
            break;
        case State.charging:
            // while charging, check if we've gone past our destination
            if((sprite.flipX && transform.position.x < chargeDestX)
                    || (!sprite.flipX && transform.position.x > chargeDestX)){
                chargeCooldownTimer = chargeCooldown;
                state = State.cooling;
                animator.SetTrigger("Stop");
            }
            break;
        case State.cooling:
            // count down the timer while not charging
            if(chargeCooldownTimer > 0){
                chargeCooldownTimer -= Time.deltaTime;
            }
            else{
                state = State.searching;
                animator.SetTrigger("Recover");
            }
            break;
        default:
            break;
        }
    }

    public void ResetChargeCD(){
        chargeCooldownTimer = chargeCooldown;
    }

    public void GetStunned(){
        StartCoroutine("StunCoroutine");
    }

    IEnumerator StunCoroutine(){
        state = State.stunned;
        animator.SetTrigger("Daze");
        enemyScript.HandleHit(40, 0);
        yield return new WaitForSeconds(2f);
        chargeCooldownTimer = chargeCooldown;
        state = State.searching;
        animator.SetTrigger("Recover");
    }

    public void DeathHandler(){
        animator.SetTrigger("Die");
        hitbox.SetActive(false);
        healthbar.SetActive(false);
    }
}
