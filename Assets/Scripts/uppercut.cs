using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uppercut : MonoBehaviour
{
    bool in_a_collision = false;
    // Start is called before the first frame update
    public GameObject slideAnimObj;
    public GameObject player;
    SpriteRenderer playerSprite;
    bool onCooldown = false;
    SpriteRenderer hitboxSprite;
    BoxCollider2D hitbox;
    public bool active;
    public Animator animator;
    Rigidbody2D rigidbody2;
    public int comboCost;

    void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
        hitboxSprite = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<BoxCollider2D>();
        rigidbody2 = player.GetComponent<Rigidbody2D>();
        active = false;
    }

    void Update()
    {
        if(!PausedGameManager.is_paused) {
            if (playerSprite.flipX)
            {
                transform.localPosition = new Vector3(-0.5f, 0.3f, 0);
                //GetComponent<Rigidbody2D>().transform.position = GetComponentInParent<Rigidbody2D>().transform.position;
            }
            else
            {
                transform.localPosition = new Vector3(0.5f, 0.3f, 0);
                //GetComponent<Rigidbody2D>().transform.position = GetComponentInParent<Rigidbody2D>().transform.position ;
            }
            if (GetComponentInParent<ComboUI>() != null){
                if (player.CompareTag("Player") && !player.GetComponent<PlayerMovement>().stunned && Input.GetKeyDown(KeyCode.Mouse1) && !onCooldown && !Input.GetKey("s") && GetComponentInParent<ComboUI>().currentCombo >= comboCost)
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
                        //hitboxSprite.enabled = true;
                        //player.GetComponent<Rigidbody2D>().velocity = (new Vector3(10000, 0, 0));
                    }
                    StartCoroutine("DashAttackCooldown");
                }
            }
        }
        
    }

    IEnumerator DashAttackCooldown()
    {
        animator.SetBool("isRunning", false);
        animator.SetTrigger("uppercut");
        onCooldown = true;
        /*if (playerSprite.flipX)
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
        }*/

        yield return new WaitForSeconds(0.4f);
        //hitbox.enabled = false;
        //hitboxSprite.enabled = false;
        yield return new WaitForSeconds(0.4f);
        onCooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player.CompareTag("Player") && other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().HandleHit(25,500);
            if (rigidbody2.velocity.y <= 0)
            {
                rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, 0);
                rigidbody2.AddForce(new Vector2(0f, 30f));
            }
            
        }
        else
        {
            if(player.CompareTag("Enemy") && other.gameObject.tag == "Player")
            {
                if (other.gameObject.GetComponent<PlayerDriver>().parrying)
                {
                    StartCoroutine("daze");
                }
                else
                {
                    other.gameObject.GetComponent<PlayerDriver>().HandleHit(5);
                }
                
            }
        }
    }
    IEnumerator daze()
    {
        player.GetComponent<platformerPathfinding>().pdazed = true;
        player.GetComponent<Animator>().SetTrigger("Dazed");
        player.GetComponent<platformerPathfinding>().pdazed = true;
        yield return new WaitForSeconds(1f);
        
        
        player.GetComponent<platformerPathfinding>().pdazed = false;
    }
}
