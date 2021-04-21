using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAttack : MonoBehaviour
{
    bool in_a_collision = false;
    // Start is called before the first frame update
    public GameObject slideAnimObj;
    public GameObject player;
    SpriteRenderer playerSprite;
    bool onCooldown = false;
    SpriteRenderer hitboxSprite;
    BoxCollider2D hitbox;
    public Animator animator;
    public Animator lowerBodyAnim;
    public int comboCost;
    float cooldown = 0.4f;
    void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
        hitboxSprite = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(!PausedGameManager.is_paused) {
            if (playerSprite.flipX)
            {
                transform.localPosition = new Vector3(-0.7f, -0.3f, 0);
                //GetComponent<Rigidbody2D>().transform.position = GetComponentInParent<Rigidbody2D>().transform.position;
            }
            else
            {
                transform.localPosition = new Vector3(0.7f, -0.3f, 0);
                //GetComponent<Rigidbody2D>().transform.position = GetComponentInParent<Rigidbody2D>().transform.position ;
            }
            if (!player.GetComponent<PlayerMovement>().stunned && Input.GetKeyDown(KeyCode.Mouse0) && !onCooldown && Input.GetKey("s") && GetComponentInParent<ComboUI>().currentCombo >= comboCost
                    && onGround())
            {

                if (playerSprite.flipX)
                {
                    //hitbox.enabled = true;
                    //hitboxSprite.enabled = true;
                    //player.GetComponent<Rigidbody2D>().velocity = (new Vector3(-10000, 0, 0));
                }
                else
                {
                    //hitbox.enabled = true;
                // hitboxSprite.enabled = true;
                    //player.GetComponent<Rigidbody2D>().velocity = (new Vector3(10000, 0, 0));
                }
                
                StartCoroutine("DashAttackCooldown");
                EventBus.Publish<MoveUsed>(new MoveUsed(cooldown,3));
            }
        }
    }

    IEnumerator DashAttackCooldown()
    {
        onCooldown = true;
        animator.Play("slide");
        lowerBodyAnim.SetTrigger("slide");
        if (playerSprite.flipX)
        {
            int i = 100;
            while (i > 75)
            {
                player.GetComponent<Rigidbody2D>().transform.position += new Vector3(-0.05f, 0, 0);
                i--;
                yield return null;
            }
            while (i > 50)
            {
                player.GetComponent<Rigidbody2D>().transform.position += new Vector3(-0.03f, 0, 0);
                i--;
                yield return null;
            }
            while (i > 0)
            {
                player.GetComponent<Rigidbody2D>().transform.position += new Vector3(-0.01f, 0, 0);
                i--;
                yield return null;
            }

        }
        else
        {
            int i = 100;
            while (i > 75)
            {
                player.GetComponent<Rigidbody2D>().transform.position += new Vector3(0.05f, 0, 0);
                i--;
                yield return null;
            }
            while (i > 50)
            {
                player.GetComponent<Rigidbody2D>().transform.position += new Vector3(0.03f, 0, 0);
                i--;
                yield return null;
            }
            while (i > 0)
            {
                player.GetComponent<Rigidbody2D>().transform.position += new Vector3(0.01f, 0, 0);
                i--;
                yield return null;
            }
        }
        
        
        //hitbox.enabled = false;
        //hitboxSprite.enabled = false;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().HandleHit(25,300);
            
        }
    }

    // check if player is currently on the ground
    bool onGround(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Terrain"));
        Debug.DrawRay(transform.position, Vector3.down * 1.1f);
        return hit.collider != null;
    }

}
