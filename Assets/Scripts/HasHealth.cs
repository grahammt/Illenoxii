using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public float maxHealth;
    public HealthBar healthBar;
    float currentHealth;
    Rigidbody2D rigidbody;
    Animator animator;
    public bool parrying = false;
    public DamageText damageTextPrefab;
    bool parrycooldown = false;
    SpriteRenderer sprite;
    public int stunNeeded = 25;
    public double currentStun = 0;
    public AudioClip hitSound;
    public AudioClip deathSound;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine("stunreset");
        if (healthBar)
            healthBar.SetMaxHealth(maxHealth);
    }
    IEnumerator stunreset()
    {
        while (true)
        {
            if (currentStun > 0)
            {
                if (currentStun > 30)
                {
                    currentStun -= currentStun*0.01f;
                }
                else
                {
                    if (currentStun > 20)
                    {
                        currentStun -= currentStun * 0.005f;
                    }
                    else
                    {
                        currentStun -= 0.1f;
                    }
                    
                }
                
            }
            yield return null;
        }
    }
    private void Update()
    {
        if(gameObject.CompareTag("Enemy"))
        {
            if(gameObject.GetComponent<platformerPathfinding>() != null )
            {
                if (currentStun > stunNeeded)
                {
                    if (animator)
                        animator.SetTrigger("Dazed");
                    gameObject.GetComponent<platformerPathfinding>().dazed = true;
                }
                else
                {
                    gameObject.GetComponent<platformerPathfinding>().dazed = false;
                }
          
            }
        }
        if (gameObject.CompareTag("Player")){
            if (currentStun > stunNeeded)
            {
                gameObject.GetComponent<PlayerMovement>().stunned = true;
                
            }
            else
            {
                gameObject.GetComponent<PlayerMovement>().stunned = false;
            }
        }
        if (gameObject.CompareTag("Player") && Input.GetKey("s") && Input.GetKeyDown(KeyCode.Mouse1) && !parrycooldown)
        {
            StartCoroutine("parry");
        }
    }
    public void takeDamage(float dmg){
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        if (!parrying){
            currentHealth -= dmg;
            if (healthBar)
                healthBar.SetCurrHealth(currentHealth);
            if (gameObject.CompareTag("Enemy"))
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, 0f);
                rigidbody.AddForce(new Vector3(0, 200, 0));

                if (damageTextPrefab)
                {
                    DamageText dmgtxt = GameObject.Instantiate(damageTextPrefab);
                    dmgtxt.damage = dmg;
                    dmgtxt.transform.position = transform.position;
                }
            }
            Debug.Log("Took " + dmg + " dmg");
            
            currentStun += dmg;
            if (gameObject.CompareTag("Player"))
            {
                currentStun += dmg;
            }
            if (currentHealth <= 0)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
                Destroy(gameObject);
            }
            if (gameObject.tag == "Player")
            {
                EventBus.Publish<ResetComboEvent>(new ResetComboEvent(0));
            }
            else if (gameObject.tag == "Enemy")
            {
                EventBus.Publish<IncrementCombo>(new IncrementCombo());
            }



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
    public void takeDamage(float dmg, float knockback)
    {
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        if (!parrying)
        {
            currentStun += dmg;
            if (gameObject.CompareTag("Player"))
            {
                currentStun += dmg;
            }
            currentHealth -= dmg;
            if (healthBar)
                healthBar.SetCurrHealth(currentHealth);
            if (gameObject.CompareTag("Enemy"))
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, 0f);
                rigidbody.AddForce(new Vector3(0, knockback, 0));
                /**if (animator)
                    animator.SetTrigger("Dazed");*/
                
                if(damageTextPrefab){
                    DamageText dmgtxt = GameObject.Instantiate(damageTextPrefab);
                    dmgtxt.damage = dmg;
                    dmgtxt.transform.position = transform.position;
                }
            }
            Debug.Log("Took " + dmg + " dmg");
            if (currentHealth <= 0)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
                Destroy(gameObject);
            }
            if (gameObject.tag == "Player" && !parrying)
            {
                EventBus.Publish<ResetComboEvent>(new ResetComboEvent(0));
            }
            else if (gameObject.tag == "Enemy")
            {
                EventBus.Publish<IncrementCombo>(new IncrementCombo());
            }
        }
        
    }
}
