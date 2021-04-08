using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial4 : MonoBehaviour
{
    public float firewallSpeed = 5f;

    void Start()
    {
        StartFireWall();
    }

    void StartFireWall(){
        foreach(Transform child in transform){
            child.GetComponent<BreakOnImpact>().sender = gameObject;
            child.GetComponent<Rigidbody2D>().velocity = Vector3.left * firewallSpeed;
        }
    }

}
