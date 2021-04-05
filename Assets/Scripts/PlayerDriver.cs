using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriver : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Animator animator;
    public bool parrying = false;
    public GameObject gameLostText;
    public SpriteRenderer lowerBody;
    bool parrycooldown = false;
    SpriteRenderer sprite;
    public int stunNeeded = 2;
    public double currentStun = 0;
    public AudioClip hitSound;
    public AudioClip deathSound;

    HasHealth healthScript;

    void Start()
    {
        healthScript = GetComponent<HasHealth>();
        StartCoroutine("stunreset");
        //StartCoroutine("test");
        Time.timeScale = 1;
    }
    /**IEnumerator test()
    {
        while (true)
        {
            lowerBody.color = new Color(0, 1f, 1, 0.5f);
            yield return new WaitForSeconds(1);
            
            lowerBody.color = new Color(1, 0.5f, 0, 0.5f);
        }
        
    }*/
    IEnumerator stunreset()
    {
        while (true)
        {
            if (currentStun > 0)
            {
                currentStun -= 0.02f;

            }
            yield return null;
        }
    }

    void Update()
    {
        Time.timeScale = 1;
        if(!PausedGameManager.is_paused) {
            if (currentStun > stunNeeded)
            {
                gameObject.GetComponent<PlayerMovement>().stunned = true;
                if (currentStun > stunNeeded*2)
                {
                    currentStun = stunNeeded*2;
                }
                
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
        lowerBody.color = new Color(0.5f, 0.5f, sprite.color.b, 0.5f);
        yield return new WaitForSeconds(1);
        parrying = false;
        sprite.color = new Color(1, 1, 1, 1);
        lowerBody.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(3);
        parrycooldown = false;
    }
}
