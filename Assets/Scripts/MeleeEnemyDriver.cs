using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyDriver : MonoBehaviour
{
    Vector3 moveVec;
    SpriteRenderer sprite;
    public float chargeDistance;

    Enemy enemyScript;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        enemyScript = GetComponentInChildren<Enemy>();

        enemyScript.deathCallBack += DeathHandler;
    }

    // interface for animation events
    public void SetChargeVectors(){
        float currChargeDist = chargeDistance;
        RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                (sprite.flipX ? Vector3.left : Vector3.right),
                chargeDistance,
                LayerMask.GetMask("Terrain"));
        if(hit.collider != null){
            currChargeDist = hit.distance;
        }
        moveVec = currChargeDist * (sprite.flipX ? Vector3.left : Vector3.right);
    }

    public void MoveTowardDest(float p){
        transform.Translate(moveVec * p);
    }

    public void DeathHandler(){
        Destroy(gameObject);
    }

}
