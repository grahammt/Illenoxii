using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*  This script will be used to manage the keyboard input for the
    player's movement.
*/ 

public class InputToPlayer : MonoBehaviour
{
    // Variable to keep track of
    private Rigidbody2D player_rb;
    private SpriteRenderer player_sprite;
    public float movement_speed = 3;
    public float jump_multiplier = 200;
    private bool in_a_collision = false;
    private bool DEBUG = true;
    private Animator animator;
    
    void Start()
    {
        player_rb = GetComponent<Rigidbody2D>();
        player_sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        // This variable will keep track of the previous velocity
        Vector2 rb_velocity = player_rb.velocity;

        // Go over horizontal player input
        if (Input.GetKey(KeyCode.D)) {
            rb_velocity.x = movement_speed;
            player_sprite.flipX = false;
        }
        else if (Input.GetKey(KeyCode.A)) {
            rb_velocity.x = movement_speed * -1;
            player_sprite.flipX = true;
        }
        else {
            rb_velocity.x = 0;
        }

        // Check for player jump
        if (Input.GetKeyDown(KeyCode.W) && in_a_collision) {
            if(DEBUG) {
                Debug.Log("Player jumping");
            }
            player_rb.AddForce(new Vector2(0, jump_multiplier));
        }

        // Give the player velocity, and set their rotation to up
        player_rb.velocity = rb_velocity;
        transform.eulerAngles = Vector3.zero;
        
    }

    public bool getOrientation() {
        return player_sprite.flipX;
    }

    // Later this will need to be tuned to take into account
    // what we are colliding with
    void OnCollisionEnter2D(Collision2D other) {
        in_a_collision = true;
    }

    void OnCollisionExit2D(Collision2D other) {
        in_a_collision = false;
    }

    void OnCollisionStay2D(Collision2D other) {
        in_a_collision = true;
    }
}
