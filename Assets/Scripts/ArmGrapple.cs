using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to go on the arm gameObject
public class ArmGrapple : MonoBehaviour
{
    public GameObject enemy_grabbed;
    public LineRenderer grapple_line;
    private Camera cam;
    private float maxRange = 12f;
    private float traveledDistance = 0;
    private float returnedDistance = 0;
    private float speedMultiplier = 40f;
    private bool onReturn = false;
    private bool onCooldown = false;

    // These variables will need to be set in "Start"
    private Vector3 returnLocation;
    private Vector3 destination;
    private Vector3 orientation;
    private Vector3 direction;
    public Transform player_transform; // set in player movement

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        returnLocation = transform.position;
        destination = cam.ScreenToWorldPoint(Input.mousePosition);
        destination.z = 0;
        direction = Vector3.Normalize(destination - returnLocation);
        AdjustRotation();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PausedGameManager.is_paused) {
            // First adjust position
            if(!onReturn) {
                transform.position += direction * speedMultiplier * Time.deltaTime;
                traveledDistance += speedMultiplier * Time.deltaTime;
                if(traveledDistance > maxRange) {
                    onReturn = true;
                }
            } else {
                // If we are on the return, we need to move towards the player
                direction = Vector3.Normalize(transform.position - player_transform.position);
                transform.position -= direction * speedMultiplier * Time.deltaTime;
                returnedDistance += speedMultiplier * Time.deltaTime;
                if(enemy_grabbed != null) {
                    enemy_grabbed.transform.position -= direction * speedMultiplier * Time.deltaTime;
                }
                if(returnedDistance > traveledDistance) {
                    if(enemy_grabbed != null) {
                        enemy_grabbed.GetComponent<Enemy>().HandleHit(5, 0);
                    }
                    EventBus.Publish(new GrappleReturnEvent());
                    Destroy(this.gameObject);
                }
            }
            // Adjust rotation of arm
            AdjustRotation();

            // Adjust trail behind
            grapple_line.SetPosition(0, transform.position);
            grapple_line.SetPosition(1, player_transform.position);
        }
    }
    
    void AdjustRotation() {
        orientation = new Vector3(0, 0, Vector3.Angle(transform.position - player_transform.position, Vector3.right));
        if (direction.y < 0) {
            orientation *= -1;
        }

        transform.eulerAngles = orientation;
    }

    void OnTriggerEnter2D(Collider2D other){
        // Make sure we're not colliding with the player
        if(!onReturn && !other.gameObject.CompareTag("Player") && other.gameObject.name != "Player") {
            Debug.Log(other.name);
            // If we hit enemy, we bring them back with us
            if(other.gameObject.CompareTag("Enemy") && other.gameObject.name != "P3_player_jump") {
                enemy_grabbed = other.gameObject;
            }
            // We don't need to do detect collisions with walls because
            // it will return by default when we set this value
            onReturn = true;
        }
    }
    
}
