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


    void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
        hitboxSprite = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<BoxCollider2D>();
        active = false;
    }

    void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.Mouse1) && !onCooldown)
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
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<HasHealth>().takeDamage(25);

        }
    }
}
