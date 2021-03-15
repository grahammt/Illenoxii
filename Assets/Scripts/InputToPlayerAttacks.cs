using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script will handle the possible attacks of the player
*/
public class InputToPlayerAttacks : MonoBehaviour
{
    public GameObject prefab;
    public bool attacking = false;
    private Camera cam;
    private float primary_attack_cooldown_MAX = 1;
    private float primary_attack_cooldown = 0;
    private float secondary_cooldown_MAX = 0.5f;
    private float secondary_cooldown = 0;
    private Vector3 primary_attack_offset = new Vector3(1, 0, 0);
    private GameObject primary_attack_prefab;
    // NOTE: we probably don't want multiple attack types at once, so
    // this can likely change to just can_attack
    private bool can_primary_attack = true;
    private bool can_shoot = true;
    private Vector3 scale_change = Vector3.right;
    public IEnumerator timer;
    void Start()
    {
        cam = Camera.main;
        timer = GetComponent<ComboTracker>().Timer();
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
            can_shoot = true;
        }

        if(can_primary_attack && Input.GetKey(KeyCode.Mouse0) && !Input.GetKey("s")) {
            PrimaryAttack();
        }
        if (can_shoot && Input.GetKeyDown(KeyCode.Mouse1)){
            SecondaryAttack();
        }       
    }

    // TODO: Make this a Co-routine
    void PrimaryAttack() {
        //TODO: Probably publish an event here
        // Set attack cooldown
        Debug.Log("Doing primary attack");
        attacking = true;
        can_primary_attack = false;
        can_shoot = false;
        primary_attack_cooldown = primary_attack_cooldown_MAX;

        // HERE IS ATTACK CODE
        // WILL LOOK DIFFERENT, PLACE PROJECTILE THERE
        bool direction = gameObject.GetComponent<InputToPlayer>().getOrientation();
        primary_attack_prefab = Instantiate(prefab);
        int temp = direction ? -1 : 1;
        primary_attack_prefab.transform.position = transform.position + primary_attack_offset * temp;
        primary_attack_prefab.GetComponent<BreakOnImpact>().player = gameObject;
        primary_attack_prefab.GetComponent<BreakOnImpact>().timer = timer;
        Physics2D.IgnoreCollision(primary_attack_prefab.GetComponent<Collider2D>(),GetComponent<Collider2D>());
    }

    void SecondaryAttack(){
        can_shoot = false;
        secondary_cooldown = secondary_cooldown_MAX;
        GameObject clone = Instantiate(prefab);
        clone.transform.position = transform.position;
        clone.GetComponent<BreakOnImpact>().player = gameObject;
        clone.GetComponent<BreakOnImpact>().timer = timer;
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
        if (attacking && other.gameObject.tag == "Enemy"){
            GetComponent<ComboTracker>().count++;
            GetComponent<ComboTracker>().SetComboText();
            StopCoroutine(timer);
            timer = GetComponent<ComboTracker>().Timer();
            StartCoroutine(timer);
        }
    }

    public void SecondaryHit(){
        GetComponent<ComboTracker>().count++;
        GetComponent<ComboTracker>().SetComboText();
        StopCoroutine(timer);
        timer = GetComponent<ComboTracker>().Timer();
        StartCoroutine(timer);
    }
}
