﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script will handle the possible attacks of the player
*/
public class InputToPlayerAttacks : MonoBehaviour
{
    public GameObject prefab;
    private Camera cam;
    private float primary_attack_cooldown_MAX = 1;
    private float primary_attack_cooldown = 0;
    // NOTE: we probably don't want multiple attack types at once, so
    // this can likely change to just can_attack
    private bool can_primary_attack = true;
    private Vector3 scale_change = Vector3.right;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Update attack cooldowns
        if(primary_attack_cooldown > 0) {
            primary_attack_cooldown -= Time.deltaTime;
        } else {
            can_primary_attack = true;
            transform.localScale = Vector3.one;

        }

        if(can_primary_attack && Input.GetKey(KeyCode.Mouse0)) {
            PrimaryAttack();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)){
            SecondaryAttack();
        }       
    }

    // TODO: Make this a Co-routine
    void PrimaryAttack() {
        //TODO: Probably publish an event here
        // Set attack cooldown
        Debug.Log("Doing primary attack");
        can_primary_attack = false;
        primary_attack_cooldown = primary_attack_cooldown_MAX;

        // HERE IS ATTACK CODE
        // WILL LOOK DIFFERENT, BUT FOR NOW JUST EXTEND SHAPE
        bool direction = gameObject.GetComponent<InputToPlayer>().getOrientation();
        float movement_modifier = direction ? -0.5f : 0.5f;
        transform.localScale += scale_change;
        transform.position = transform.position + new Vector3(movement_modifier, 0, 0);
    }

    void SecondaryAttack(){
        GameObject clone = Instantiate(prefab);
        clone.transform.position = transform.position;
        Physics2D.IgnoreCollision(clone.GetComponent<Collider2D>(),GetComponent<Collider2D>());
        Vector2 mousePos = new Vector2();
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
        Vector3 angle = Vector3.Normalize(cam.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y,0.0f))-transform.position);
        angle.z = 0.0f;
        Debug.Log(angle);
        clone.GetComponent<Rigidbody2D>().velocity = angle * 5;
    }
}
