using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameHitbox : MonoBehaviour
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
        active = true;
    }

    void Update()
    {
        if (!PausedGameManager.is_paused)
        {
            if (playerSprite.flipX)
            {
                transform.localPosition = new Vector3(-0.3f, 0.31f, 0);
                hitboxSprite.flipX = true;
                //GetComponent<Rigidbody2D>().transform.position = GetComponentInParent<Rigidbody2D>().transform.position;
            }
            else
            {
                transform.localPosition = new Vector3(0.3f, 0.31f, 0);
                hitboxSprite.flipX = false;
                //GetComponent<Rigidbody2D>().transform.position = GetComponentInParent<Rigidbody2D>().transform.position ;
            }
            
        }

    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player.CompareTag("Enemy") && other.gameObject.tag == "Player")
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
    IEnumerator daze()
    {
        player.GetComponent<platformerPathfinding>().pdazed = true;
        player.GetComponent<Animator>().SetBool("Dazed", true);
        player.GetComponent<platformerPathfinding>().pdazed = true;
        yield return new WaitForSeconds(1f);
        player.GetComponent<Animator>().SetBool("Dazed", false);

        player.GetComponent<platformerPathfinding>().pdazed = false;
    }
}
