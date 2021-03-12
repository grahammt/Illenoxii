using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToTarget : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    void Start(){
        offset = transform.position - target.position;
    }

    void Update()
    {
        transform.position = target.position + offset;
    }
}
