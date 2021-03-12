using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{

    public GameObject dashAnimObj;
    Animator dashAnim;
    Transform dashAnimTf;
    SpriteRenderer playerSprite;
    bool onCooldown = false;

    void Start(){
        dashAnim = dashAnimObj.GetComponent<Animator>();
        dashAnimTf = dashAnimObj.transform;
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && !onCooldown){
            transform.Translate(new Vector3(playerSprite.flipX ? -4f : 4f, 0f, 0f));
            dashAnim.SetTrigger("Dash");
            dashAnimTf.position = transform.position + new Vector3(playerSprite.flipX ? 4f : -4f, 0f, 0f);
            StartCoroutine("DashAttackCooldown");
        }
    }

    IEnumerator DashAttackCooldown(){
        onCooldown = true;
        yield return new WaitForSeconds(0.8f);
        onCooldown = false;
    }
}
