using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargetWithinRange : MonoBehaviour
{
    public Transform target;
    public float attackRange;

    Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!PausedGameManager.is_paused) { 
            float distanceFromTarget = Vector3.Distance(transform.position, target.position);
            if(distanceFromTarget < 1f){
                animator.SetTrigger("Attack");
            }
        }

    }
}
