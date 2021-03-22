using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryAttack : MonoBehaviour
{
    public Animator playerAnim;
    public GameObject player;
    SpriteRenderer playerSprite;
    BoxCollider2D hitbox;
    bool onCooldown = false;

    void Start(){
        playerSprite = player.GetComponent<SpriteRenderer>();
        hitbox = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(playerSprite.flipX) {
            transform.localPosition = new Vector3(-0.5f, 0.3f, 0);
        }
        else {
            transform.localPosition = new Vector3(0.5f, 0.3f, 0);
        }
        // TODO: Animate the attack
        if(Input.GetKeyDown(KeyCode.Mouse0) && !onCooldown){
             StartCoroutine("PrimaryAttackCooldown");
        }
    }

    IEnumerator PrimaryAttackCooldown(){
        // TODO: Set animation flags
        playerAnim.SetBool("isRunning", false);
        playerAnim.SetTrigger("punch");
        onCooldown = true;
        hitbox.enabled = true;
        yield return new WaitForSeconds(0.6f);
        hitbox.enabled = false;
        onCooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            other.gameObject.GetComponent<HasHealth>().takeDamage(10);
        }
    }

}
