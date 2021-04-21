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

    public bool dash_lock = false;
    public bool grapple_lock = false;

    // components to cache
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSprite;
    private Animator playerAnim;
    public Animator lowerBodyAnim;
    public SpriteRenderer lowerBodySprite;
    public bool stunned = false;
    bool colorRed = false;
    bool colorBlue = false;
    bool inMidair = true;

    // state variables
    public enum movementState{idle, running, dashing, grapple};
    movementState state = movementState.idle;
    float direction;
    public AudioSource walkSound;

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
    public int dashComboCost;
    public AudioClip dashSound;

    // Variables for grapple
    public int grappleComboCost = 5;
    private float grappleCooldown = 3f;
    private bool onGrappleCooldown = false;
    public AudioClip grappleSound;

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

        // Grapple event setup
        EventBus.Subscribe<GrappleReturnEvent>(_GrappleReturn);
    }

    // Update is called once per frame
    void Update()
    {
        if(!PausedGameManager.is_paused) {
            if (gameObject.GetComponentInChildren<PlayerDriver>().parrying)
            {
                colorBlue = true;
                lowerBodySprite.color = new Color(0.5f, 0.5f, 1, 0.5f);
            }
            else
            {
                if (colorBlue)
                {
                    colorBlue = false;
                    lowerBodySprite.color = new Color(1, 1, 1, 1);
                }
            }
            switch(state){
                case movementState.idle:
                    StopRunSound();
                    checkMovement();
                    break;
                case movementState.running:
                    playerRb.velocity = new Vector2(direction * movement_speed, playerRb.velocity.y);
                    // Set running sound
                    EnableRunSound();
                    checkMovement();
                    break;
                case movementState.dashing:
                    StopRunSound();
                    break;
                default:
                    StopRunSound();
                    break;
            }
            if (stunned)
            {
                playerAnim.SetBool("isRunning", true);
                //lowerBodyAnim.SetBool("isMidair", true);
                playerSprite.color = new Color(1, 0.5f*playerSprite.color.g, 0.5f*playerSprite.color.b);
                lowerBodySprite.color = new Color(1, 0.5f * playerSprite.color.g, 0.5f * playerSprite.color.b);
                colorRed = true;
            }
            else
            {
                if (colorRed)
                {
                    playerSprite.color = new Color(1, 1, 1);
                    lowerBodySprite.color = new Color(1, 1, 1);
                    colorRed = false;
                }
                
                /**if (onGround())
                {
                    lowerBodyAnim.SetBool("isMidair", false);
                }*/
            }
            {
                
            }
        }
    }

    void FixedUpdate(){
        switch(state){
            case movementState.idle:
                break;
            case movementState.running:
                //playerRb.velocity = new Vector2(direction * movement_speed, playerRb.velocity.y);
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
                lowerBodySprite.flipX = false;
                state = movementState.running;
                direction = 1f;
                playerAnim.SetBool("isRunning", true);
                lowerBodyAnim.SetBool("isRunning", true);
            }
            else if(Input.GetKey(KeyCode.A)){
                playerSprite.flipX = true;
                lowerBodySprite.flipX = true;
                state = movementState.running;
                direction = -1f;
                playerAnim.SetBool("isRunning", true);
                lowerBodyAnim.SetBool("isRunning", true);
            }
            else {
                playerRb.velocity = new Vector2(0f, playerRb.velocity.y);
                state = movementState.idle;
                playerAnim.SetBool("isRunning", false);
                lowerBodyAnim.SetBool("isRunning", false);
            }
        }

        // then check dash command
        if(!dash_lock && !stunned && dashCharges > 0 && Input.GetKeyDown(KeyCode.LeftShift) && GetComponent<ComboUI>().currentCombo >= dashComboCost){
            // get mouse position, set z component to 0
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos[2] = 0f;

            // calculate the travel vector
            dashStart = transform.position;
            Vector3 dashVec = Vector3.ClampMagnitude(mousePos - dashStart, dashDistance);
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.8f, dashVec.normalized, dashVec.magnitude, LayerMask.GetMask("Terrain"));
            

            if (hit.collider != null){ // a wall is in the way of the dash
                dashVec = new Vector3(hit.centroid.x,hit.centroid.y,0) - transform.position;
                
            }
            dashDest = dashStart + dashVec;

            // set various state variables
            AudioSource.PlayClipAtPoint(dashSound, transform.position);
            state = movementState.dashing;
            playerSprite.flipX = dashDest.x < dashStart.x;
            lowerBodySprite.flipX = dashDest.x < dashStart.x;

            // message the animator
            playerAnim.SetBool("isRunning", false);
            lowerBodyAnim.SetBool("isRunning", false);
            playerAnim.SetTrigger("Dash");
            lowerBodyAnim.SetBool("isMidair", true);
            playerAnim.SetBool("inMidair", true);
            inMidair = true;

            // update the dash trail
            // look for an available dash charge
            for(int i = 0; i < dashes.Length; ++i){
                if(!dashes[i].onCooldown){
                    float angle = Vector3.SignedAngle(Vector3.right, dashVec, Vector3.forward);
                    if(angle < 0){
                        angle += 360;
                    }
                    dashes[0].tf.localScale = new Vector3(dashVec.magnitude, 2f, 0);
                    dashes[0].tf.eulerAngles = new Vector3(0f, 0f, angle);
                    dashes[0].tf.position = transform.position + dashVec / 2;
                    dashes[0].anim.SetTrigger("Dash");
                    // dashes[0].onCooldown = true;
                    // --dashCharges;
                    // coroutine to handle things past the setup
                    StartCoroutine("DashAttackMain", i);
                    break;
                }
            }
        }

        // check grapple
        if (!grapple_lock && !stunned && Input.GetKeyDown(KeyCode.Q) && GetComponent<ComboUI>().currentCombo >= grappleComboCost && !onGrappleCooldown) {
            // set various state variables
            AudioSource.PlayClipAtPoint(grappleSound, transform.position);
            state = movementState.grapple;
            Vector3 grappleOffset = Vector3.zero;// getOrientation() ? new Vector3(-.5f, 0, 0) : new Vector3(.5f, 0, 0);
            // playerSprite.flipX = dashDest.x < dashStart.x;

            // message the animator
            playerAnim.SetBool("isRunning", false);
            lowerBodyAnim.SetBool("isRunning", false);
            playerAnim.SetBool("grapple", true);
            // playerAnim.SetTrigger("Dash");
            // lowerBodyAnim.SetBool("isMidair", true);

            // spawn the grappler
            GameObject grapObj = Resources.Load<GameObject>("Grapple");
            GameObject grapple = Instantiate(grapObj, transform.position + grappleOffset, Quaternion.identity);
            grapple.GetComponent<ArmGrapple>().player_transform = gameObject.transform;
            StartCoroutine("GrappleMain");

        }

        // then check for player jump
        if (Input.GetKeyDown(KeyCode.Space) && onGround()) {
            playerRb.AddForce(new Vector2(0, jump_multiplier));
            lowerBodyAnim.SetBool("isMidair", true);
            playerAnim.SetBool("inMidair", true);
            inMidair = true;
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
            if(onGround()){
                lowerBodyAnim.SetBool("isMidair", false);
                playerAnim.SetBool("inMidair", false);
                inMidair = false;
            }
        }
    }

    void OnCollisionStay2D(Collision2D other){
        if(inMidair){
            if(other.gameObject.CompareTag("Terrain")){
                if(onGround()){
                    lowerBodyAnim.SetBool("isMidair", false);
                    playerAnim.SetBool("inMidair", false);
                    inMidair = false;
                }
            }
        }
    }

    IEnumerator DashAttackMain(int dashIdx){
        float dashCooldown = 2.4f;
        dashCharges--;
        EventBus.Publish<MoveUsed>(new MoveUsed(dashCooldown,dashIdx));
        dashes[dashIdx].onCooldown = true;
        yield return new WaitForSeconds(dashCooldown);
        dashes[dashIdx].onCooldown = false;
        dashCharges++;
    }

    IEnumerator GrappleMain() {
        onGrappleCooldown = true;
        EventBus.Publish<MoveUsed>(new MoveUsed(grappleCooldown,2));
        yield return new WaitForSeconds(grappleCooldown);
        onGrappleCooldown = false;
    }

    void _GrappleReturn(GrappleReturnEvent e) {
        playerAnim.SetBool("grapple", false);
        state = movementState.idle;
    }

    void MoveToDashDest(float prog){
        transform.position = Vector3.Lerp(dashStart, dashDest, prog);
    }

    void EndDash(){
        state = movementState.idle;
        playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
    }

    public void StopRunSound() {
        walkSound.enabled = false;
    }

    void EnableRunSound() {
        walkSound.enabled = (inMidair) ? false : true;
    }
}

public class MoveUsed{
    public float cooldown;
    public int move; // 0/1 for dash, 2 for grapple, 3 for slide, 4 for uppercut
    public MoveUsed(){}
    public MoveUsed(float cooldownLength, int moveID){
        cooldown = cooldownLength;
        move = moveID;
    }
}
