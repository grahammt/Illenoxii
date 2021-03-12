using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*  This script will be used to manage the keyboard input for the
    player's movement.
*/ 

public class InputToPlayer : MonoBehaviour
{
    // Variable to keep track of
    private Rigidbody player_rb;
    public float movement_speed = 3;
    public float jump_multiplier = 200;
    private bool in_a_collision = false;
    private bool DEBUG = true;
    // false is left, true is right
    public bool orientation = true;
    void Start()
    {
        player_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // This variable will keep track of the previous velocity
        Vector2 rb_velocity = player_rb.velocity;

        // Go over horizontal player input
        if (Input.GetKey(KeyCode.D)) {
            rb_velocity.x = movement_speed;
            orientation = true;
        }
        else if (Input.GetKey(KeyCode.A)) {
            rb_velocity.x = movement_speed * -1;
            orientation = false;
        }
        else {
            rb_velocity.x = 0;
        }

        // Check for player jump
        if (Input.GetKey(KeyCode.W) && in_a_collision) {
            if(DEBUG) {
                Debug.Log("Player jumping");
            }
            player_rb.AddForce(new Vector2(0, jump_multiplier));
        }

        // Give the player velocity, and set there rotation to up
        player_rb.velocity = rb_velocity;
        transform.eulerAngles = Vector3.zero;

    }

    public bool getOrientation() {
        return orientation;
    }

    // Later this will need to be tuned to take into account
    // what we are colliding with
    void OnCollisionEnter(Collision other) {
        in_a_collision = true;
    }

    void OnCollisionExit(Collision other) {
        in_a_collision = false;
    }

    void OnCollisionStay(Collision other) {
        in_a_collision = true;
    }
}
