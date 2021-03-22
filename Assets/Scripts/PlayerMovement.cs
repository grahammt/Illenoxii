using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // inspector variables
    public float movement_speed = 3;
    public float dashDistance = 5;
    public float jump_multiplier = 200;

    // components to cache
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSprite;
    private Animator playerAnim;

    // state variables
    public enum movementState{idle, running, dashing};
    movementState state = movementState.idle;
    float direction;

    // imported from DashAttack.cs
    struct dashStruct{
        public Animator anim;
        public Transform tf;
        public int idx;
        public bool onCooldown;
    };
    dashStruct[] dashes = new dashStruct[2];
    public GameObject dashTrail1;
    public GameObject dashTrail2;
    int dashCharges = 2;
    Camera cam;
    Vector3 dashStart;
    Vector3 dashDest;

    void Start(){
        // caching components
        playerRb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();

        // imported from DashAttack.cs
        cam = Camera.main;
        dashes[0].anim = dashTrail1.GetComponentInChildren<Animator>();
        dashes[0].tf = dashTrail1.transform;
        dashes[0].idx = 0;
        dashes[1].anim = dashTrail2.GetComponentInChildren<Animator>();
        dashes[1].tf = dashTrail2.transform;
        dashes[1].idx = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state){
            case movementState.idle:
                checkMovement();
                break;
            case movementState.running:
                checkMovement();
                break;
            case movementState.dashing:
                break;
            default:
                break;
        }

        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void FixedUpdate(){
        switch(state){
            case movementState.idle:
                break;
            case movementState.running:
                playerRb.velocity = new Vector2(direction * movement_speed, playerRb.velocity.y);
                break;
            case movementState.dashing:
                break;
            default:
                break;
        }
    }

    public bool getOrientation() {
        return playerSprite.flipX;
    }

    void checkMovement()
    {
        // first check horizontal input
        {
            // Go over horizontal player input
            if (Input.GetKey(KeyCode.D)) {
                playerSprite.flipX = false;
                state = movementState.running;
                direction = 1f;
                playerAnim.SetBool("isRunning", true);
            }
            else if(Input.GetKey(KeyCode.A)){
                playerSprite.flipX = true;
                state = movementState.running;
                direction = -1f;
                playerAnim.SetBool("isRunning", true);
            }
            else {
                playerRb.velocity = new Vector2(0f, playerRb.velocity.y);
                state = movementState.idle;
                playerAnim.SetBool("isRunning", false);
            }
        }

        // then check dash command
        if(dashCharges > 0 && Input.GetKeyDown("e")){
            // get mouse position, set z component to 0
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos[2] = 0f;

            // calculate the travel vector
            dashStart = transform.position;
            Vector3 dashVec = Vector3.ClampMagnitude(mousePos - dashStart, dashDistance);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dashVec.normalized, dashVec.magnitude, LayerMask.GetMask("Terrain"));
            if(hit.collider != null){ // a wall is in the way of the dash
                dashVec = Vector3.ClampMagnitude(dashVec, hit.distance - 0.8f);
            }
            dashDest = dashStart + dashVec;

            // set various state variables
            state = movementState.dashing;
            playerSprite.flipX = dashDest.x < dashStart.x;

            // message the animator
            playerAnim.SetBool("isRunning", false);
            playerAnim.SetTrigger("Dash");

            // update the dash trail
            // look for an available dash charge
            for(int i = 0; i < dashes.Length; ++i){
                if(!dashes[i].onCooldown){
                    float angle = Vector3.SignedAngle(Vector3.right, dashVec, Vector3.forward);
                    if(angle < 0){
                        angle += 360;
                    }
                    dashes[0].tf.eulerAngles = new Vector3(0f, 0f, angle);
                    dashes[0].tf.position = transform.position;
                    dashes[0].anim.SetTrigger("Dash");
                    // dashes[0].onCooldown = true;
                    // --dashCharges;
                    // coroutine to handle things past the setup
                    StartCoroutine("DashAttackMain", 0);
                    break;
                }
            }
        }

        // then check for player jump
        if (Input.GetKeyDown(KeyCode.Space) && onGround()) {
            playerRb.AddForce(new Vector2(0, jump_multiplier));
        }
    }

    // check if player is currently on the ground
    bool onGround(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Terrain"));
        Debug.DrawRay(transform.position, Vector3.down * 1.1f);
        return hit.collider != null;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Terrain"){
            // dashes[0].onCooldown = false;
            // dashes[1].onCooldown = false;
            // dashCharges = 2;
        }
    }

    IEnumerator DashAttackMain(int dashIdx){
        dashCharges--;
        dashes[dashIdx].onCooldown = true;
        yield return new WaitForSeconds(2.4f);
        dashes[dashIdx].onCooldown = false;
        dashCharges++;
    }

    void MoveToDashDest(float prog){
        transform.position = Vector3.Lerp(dashStart, dashDest, prog);
    }

    void EndDash(){
        state = movementState.idle;
        playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
    }


}
