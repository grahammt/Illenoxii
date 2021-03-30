using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyDriver : MonoBehaviour
{
    Vector3 startVec;
    Vector3 destVec;
    SpriteRenderer sprite;
    public float chargeDistance;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // interface for animation events
    public void SetChargeVectors(){
        startVec = transform.position;
        destVec = startVec + chargeDistance * (sprite.flipX ? Vector3.left : Vector3.right);
    }

    public void LerpToDest(float p){
        transform.position = Vector3.Lerp(startVec, destVec, p);
    }

}
