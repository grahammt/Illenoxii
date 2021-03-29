using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public Vector3 startVec;
    public Vector3 destVec;
    SpriteRenderer sprite;
    public float chargeDistance;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetChargeVectors(){
        startVec = transform.position;
        destVec = startVec + chargeDistance * (sprite.flipX ? Vector3.left : Vector3.right);
    }

    public void LerpToDest(float p){
        transform.position = Vector3.Lerp(startVec, destVec, p);
    }

}
