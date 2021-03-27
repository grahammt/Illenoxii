using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverAfterFall : MonoBehaviour
{
    public bool dazed;
    Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
    }

    void PathfindingSetDazed(){
        dazed = true;
    }

    void OnCollisionEnter2D(Collision2D other){
        if(dazed){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.6f, LayerMask.GetMask("Terrain"));
            Debug.DrawRay(transform.position, Vector2.down * 1.4f);
            if(hit.collider != null){
                dazed = false;
                animator.SetTrigger("Recover");
            }
        }
    }
}
