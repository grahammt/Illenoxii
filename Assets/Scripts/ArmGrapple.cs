using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to go on the arm gameObject
public class ArmGrapple : MonoBehaviour
{
    public GameObject enemy_grabbed;
    private Camera cam;
    public float maxRange = 7f;
    private float traveledDistance = 0;
    private float returnedDistance = 0;
    private float speedMultiplier = 20f;
    private bool onReturn = false;
    private bool onCooldown = false;

    // These variables will need to be set in "Start"
    private Vector3 returnLocation;
    private Vector3 destination;
    private Vector3 orientation;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        returnLocation = transform.position;
        destination = cam.ScreenToWorldPoint(Input.mousePosition);
        direction = Vector3.Normalize(destination - returnLocation);
        orientation = new Vector3(0, 0, Vector3.Angle(destination - returnLocation, transform.right));
        if (direction.y < 0) {
            orientation *= -1;
        }

        transform.eulerAngles = orientation;

    }

    // Update is called once per frame
    void Update()
    {
        if(!PausedGameManager.is_paused) {
            if(!onReturn) {
                transform.position += direction * speedMultiplier * Time.deltaTime;
                traveledDistance += speedMultiplier * Time.deltaTime;
                if(traveledDistance > maxRange) {
                    onReturn = true;
                }
            } else {
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
        }
    }
    
    void OnTriggerEnter2D(Collider2D other){
        // Make sure we're not colliding with the player
        if(!onReturn && !other.gameObject.CompareTag("Player")) {
            // If we hit enemy, we bring them back with us
            if(other.gameObject.CompareTag("Enemy")) {
                enemy_grabbed = other.gameObject;
            }
            // We don't need to do detect collisions with walls because
            // it will return by default when we set this value
            onReturn = true;
        }

        
    }
}
