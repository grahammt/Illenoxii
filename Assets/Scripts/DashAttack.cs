using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{

    public float dashDistance;
    public GameObject dashTrail;
    Animator dashTrailAnim;
    Animator playerAnim;
    Transform dashTrailTf;
    Rigidbody2D playerRb;
    SpriteRenderer playerSprite;
    bool onCooldown = false;
    Camera cam;

    void Start(){
        cam = Camera.main;
        dashTrailAnim = dashTrail.GetComponent<Animator>();
        dashTrailTf = dashTrail.transform;
        playerAnim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && !onCooldown){
            //transform.Translate(new Vector3(playerSprite.flipX ? -4f : 4f, 0f, 0f));
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            playerRb.velocity = mousePos.normalized * dashDistance * 10;
            playerSprite.flipX = mousePos.x < 0;
            StartCoroutine("ResetSpeedAfterDelay", 0.2f);
            dashTrailAnim.SetTrigger("Dash");
            playerAnim.SetTrigger("Dash");
            dashTrailTf.position = transform.position + new Vector3(playerSprite.flipX ? -2f : 2f, 0f, 0f);
            StartCoroutine("DashAttackCooldown");
        }
    }

    IEnumerator DashAttackCooldown(){
        onCooldown = true;
        yield return new WaitForSeconds(0.8f);
        onCooldown = false;
    }

    IEnumerator ResetSpeedAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        playerRb.velocity = Vector3.zero;
    }
}
