using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriver : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Animator animator;
    public bool parrying = false;
    public GameObject gameLostText;
    bool parrycooldown = false;
    SpriteRenderer sprite;
    public int stunNeeded = 25;
    public double currentStun = 0;
    public AudioClip hitSound;
    public AudioClip deathSound;

    HasHealth healthScript;

    void Start()
    {
        healthScript = GetComponent<HasHealth>();
        
    }

    void Update()
    {
        if(!PausedGameManager.is_paused) {
            if (currentStun > stunNeeded)
            {
                gameObject.GetComponent<PlayerMovement>().stunned = true;
                
            }
            else
            {
                gameObject.GetComponent<PlayerMovement>().stunned = false;
            }
            if (Input.GetKey("s") && Input.GetKeyDown(KeyCode.Mouse1) && !parrycooldown)
            {
                StartCoroutine("parry");
            }
        }
    }

    public void HandleHit(float dmg){
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        if (!parrying){
            bool dead = healthScript.TakeDamage(dmg);

            currentStun += dmg;
            if (dead)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
                // game lost
                gameLostText.SetActive(true);
                Destroy(gameObject);
            }
            EventBus.Publish<ResetComboEvent>(new ResetComboEvent(0));
        }
    }

    IEnumerator parry()
    {
        parrying = true;
        parrycooldown = true;
        sprite.color = new Color(0.5f, 0.5f, sprite.color.b, 0.5f);
        yield return new WaitForSeconds(1);
        parrying = false;
        sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(3);
        parrycooldown = false;
    }
}
