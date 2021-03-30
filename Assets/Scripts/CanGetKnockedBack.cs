using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanGetKnockedBack : MonoBehaviour
{
    Rigidbody2D rigidbody;

    void Start(){
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void knockback(float force){
        // first reset vertical momentum
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, 0f);

        // then add upward force
        rigidbody.AddForce(new Vector3(0, force, 0));

    }

}
