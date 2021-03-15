﻿using System.Collections;
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

    void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
        hitboxSprite = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
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
        if (Input.GetKeyDown("x") && !onCooldown && Input.GetKey("s") && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) <=0.001)
        {

            if (playerSprite.flipX)
            {
                hitbox.enabled = true;
                hitboxSprite.enabled = true;
                //player.GetComponent<Rigidbody2D>().velocity = (new Vector3(-10000, 0, 0));
            }
            else
            {
                hitbox.enabled = true;
                hitboxSprite.enabled = true;
                //player.GetComponent<Rigidbody2D>().velocity = (new Vector3(10000, 0, 0));
            }
            
            StartCoroutine("DashAttackCooldown");
        }
    }

    IEnumerator DashAttackCooldown()
    {
        onCooldown = true;
        if (playerSprite.flipX)
        {
            int i = 100;
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
            while (i > 0)
            {
                player.GetComponent<Rigidbody2D>().transform.position += new Vector3(0.01f, 0, 0);
                i--;
                yield return null;
            }
        }
        
        
        hitbox.enabled = false;
        hitboxSprite.enabled = false;
        yield return new WaitForSeconds(0.4f);
        onCooldown = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        in_a_collision = true;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        in_a_collision = false;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        in_a_collision = true;
    }
}