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
    Rigidbody2D rigidbody;
    void Start(){
        playerSprite = player.GetComponent<SpriteRenderer>();
        rigidbody = player.GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(!PausedGameManager.is_paused) {
            if(playerSprite.flipX) {
                transform.localPosition = new Vector3(-0.5f, 0.3f, 0);
            }
            else {
                transform.localPosition = new Vector3(0.5f, 0.3f, 0);
            }
            // TODO: Animate the attack
            if(!player.GetComponent<PlayerMovement>().stunned && Input.GetKeyDown(KeyCode.Mouse0) && !onCooldown && !Input.GetKey("s")){
                StartCoroutine("PrimaryAttackCooldown");
            }
        }
    }

    IEnumerator PrimaryAttackCooldown(){
        // TODO: Set animation flags
        playerAnim.SetBool("isRunning", false);
        playerAnim.SetTrigger("punch");
        onCooldown = true;
        //hitbox.enabled = true;
        yield return new WaitForSeconds(0.2f);
        //hitbox.enabled = false;
        onCooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy>().HandleHit(5,5);
            if (rigidbody.velocity.y <= 0)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                rigidbody.AddForce(new Vector2(0f, 20f));
            }
            
        }
        
    }

}
