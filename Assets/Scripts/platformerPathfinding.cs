using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class platformerPathfinding : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 3f;
    public float nextWaypointDistance = 2f;
    public float jumpMultiplier = 300f;
    public bool dazed = false;


    Rigidbody2D rigidbody;
    Animator animator;
    Path path;
    Seeker seeker;
    bool reachedEndOfPath = true;
    int currentWaypoint = 0;
    bool jumping = false;
    SpriteRenderer spriteR;
    void Start(){
        seeker = GetComponent<Seeker>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        InvokeRepeating("RecalculatePath", 1f, 0.5f);
        spriteR = GetComponent<SpriteRenderer>();
    }

    void RecalculatePath(){
        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    void PathfindingSetDazed(){
        dazed = true;
    }

    void PathfindingSetUnDazed(){
        dazed = false;
    }

    void OnPathComplete (Path p) {
        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    void Update(){
        if(dazed){
            return;
        }
        float distanceFromTarget = Vector3.Distance(transform.position, target.position);
        if(distanceFromTarget < 2f && distanceFromTarget >0.8f){
            animator.SetTrigger("uppercut");
            return;
        }
        if(path == null){
            return;
        }
        
        reachedEndOfPath = false;
        float distanceToWaypoint;
        while (true) {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Mathf.Abs(transform.position.x - path.vectorPath[currentWaypoint].x);
            if (distanceToWaypoint < nextWaypointDistance) {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                } else {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            } else {
                break;
            }
        }

        //jumping = rigidbody.velocity.y > 0.2f;

        Vector3 dest = path.vectorPath[currentWaypoint];
        if(dest.x < transform.position.x){
            //rigidbody.velocity = new Vector3(-moveSpeed, rigidbody.velocity.y, 0f);
            spriteR.flipX = true;
            if (distanceFromTarget < 0.8f)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * -moveSpeed * Time.deltaTime);
            }
            
        }
        else if(dest.x > transform.position.x){
            //rigidbody.velocity = new Vector3(moveSpeed, rigidbody.velocity.y, 0f);
            spriteR.flipX = false;
            if (distanceFromTarget < 0.8f)
            {
                transform.Translate(Vector3.right * -moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            
        }
        /*if(dest.y > transform.position.y && !jumping){
            rigidbody.AddForce(new Vector2(0f, jumpMultiplier));
            jumping = true;
        }*/

    }
}
