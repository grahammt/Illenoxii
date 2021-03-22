using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script will handle the possible attacks of the player
*/
public class InputToPlayerAttacks : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public GameObject prefab;
    public bool attacking = false;
    private Camera cam;
    private float primary_attack_cooldown_MAX = 1;
    private float primary_attack_cooldown = 0;
    private float secondary_cooldown_MAX = 1;
    private float secondary_cooldown = 0;
    private Vector3 primary_attack_offset = new Vector3(1, 0, 0);
    private GameObject primary_attack_prefab;
    // NOTE: we probably don't want multiple attack types at once, so
    // this can likely change to just can_attack
    private bool can_primary_attack = true;
    private bool can_secondary = true;
    private Vector3 scale_change = Vector3.right;
    //public IEnumerator timer;
    void Start()
    {
        cam = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        //timer = GetComponent<ComboTracker>().Timer();
    }

    void Update()
    {
        // Update attack cooldowns
        if(primary_attack_cooldown > 0) {
            primary_attack_cooldown -= Time.deltaTime;
        } else {
            attacking = false;
            can_primary_attack = true;
            Destroy(primary_attack_prefab);
        }

        if (secondary_cooldown > 0){
            secondary_cooldown -=Time.deltaTime;
        }
        else{
            can_secondary = true;
        }

        if(can_primary_attack && Input.GetKey(KeyCode.Mouse0) && !Input.GetKey("s")) { 
            // if (GetComponent<Rigidbody2D>().velocity.y != 0){
            //     GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,0);
            // }
            // PrimaryAttack();
            rigidbody.velocity = new Vector2(rigidbody.velocity.x,0);
            rigidbody.AddForce(new Vector2(0f, 30f));
        }
        if (can_secondary && Input.GetKeyDown(KeyCode.Mouse1)){
            //if (rigidbody.velocity.y != 0){
            rigidbody.velocity = new Vector2(rigidbody.velocity.x,0);
            rigidbody.AddForce(new Vector2(0f, 30f));
            //}
            SecondaryAttack();
        }       
    }

    // // TODO: Make this a Co-routine
    // void PrimaryAttack() {
    //     //TODO: Probably publish an event here
    //     // Set attack cooldown
    //     Debug.Log("Doing primary attack");
    //     attacking = true;
    //     can_primary_attack = false;
    //     can_secondary = false;
    //     primary_attack_cooldown = primary_attack_cooldown_MAX;

    //     // HERE IS ATTACK CODE
    //     // WILL LOOK DIFFERENT, PLACE PROJECTILE THERE
    //     bool direction = gameObject.GetComponent<InputToPlayer>().getOrientation();
    //     primary_attack_prefab = Instantiate(prefab);
    //     int temp = direction ? -1 : 1;
    //     primary_attack_prefab.transform.position = transform.position + primary_attack_offset * temp;
    //     primary_attack_prefab.GetComponent<BreakOnImpact>().sender = gameObject;
    //     primary_attack_prefab.GetComponent<BreakOnImpact>().damage = 25;
    //     Physics2D.IgnoreCollision(primary_attack_prefab.GetComponent<Collider2D>(),GetComponent<Collider2D>());
    // }

    void SecondaryAttack(){
        
    }

    void FireProjectile(){
        can_secondary = false;
        secondary_cooldown = secondary_cooldown_MAX;
        GameObject clone = Instantiate(prefab);
        clone.transform.position = transform.position;
        clone.GetComponent<BreakOnImpact>().sender = gameObject;
        Physics2D.IgnoreCollision(clone.GetComponent<Collider2D>(),GetComponent<Collider2D>());
        Vector2 mousePos = new Vector2();
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y,cam.nearClipPlane));
        Vector3 angle = Vector3.Normalize(new Vector3(point.x-transform.position.x, point.y-transform.position.y, 0.0f));
        Debug.Log(angle);
        clone.GetComponent<Rigidbody2D>().velocity = angle * 5;
    }

    void OnCollisionEnter2D(Collision2D other){
        /*if (attacking && other.gameObject.tag == "Enemy"){
            GetComponent<ComboTracker>().count++;
            GetComponent<ComboTracker>().SetComboText();
            StopCoroutine(timer);
            timer = GetComponent<ComboTracker>().Timer();
            StartCoroutine(timer);
        }*/
    }

    /*public void SecondaryHit(){
        GetComponent<ComboTracker>().count++;
        GetComponent<ComboTracker>().SetComboText();
        StopCoroutine(timer);
        timer = GetComponent<ComboTracker>().Timer();
        StartCoroutine(timer);
    }*/
}
