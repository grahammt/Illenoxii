using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for storing unused code
// be sure to add comments labeling the script that the code snippet belongs to

    /*
    // Code for player to create a projectile and fire it in direction of mouse 
    void FireProjectile(){
        can_secondary = false;
        secondary_cooldown = secondary_cooldown_MAX;
        GameObject clone = Instantiate(prefab);
        clone.transform.position = transform.position;
        clone.GetComponent<BreakOnImpact>().sender = gameObject;
        Physics2D.IgnoreCollision(clone.GetComponent<Collider2D>(),GetComponent<Collider2D>());
        Vector2 mousePos = new Vector2();
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y,cam.nearClipPlane));
        Vector3 angle = Vector3.Normalize(new Vector3(point.x-transform.position.x, point.y-transform.position.y, 0.0f));
        Debug.Log(angle);
        clone.GetComponent<Rigidbody2D>().velocity = angle * 5;
    }*/