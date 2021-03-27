using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SimpleChaseTarget : StateMachineBehaviour
{
    public float moveSpeed = 3f;
    public float nextWaypointDistance = 2f;

    Transform transform;
    Rigidbody2D rigidbody;
    platformerPathfinding pathfinder;
    Path path;
    bool reachedEndOfPath = true;
    int currentWaypoint = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform = animator.transform;
        rigidbody = animator.GetComponent<Rigidbody2D>();
        pathfinder = animator.GetComponent<platformerPathfinding>();
        path = pathfinder.path;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(path == null){
            return;
        }
        else if(path != pathfinder.path){
            path = pathfinder.path;
            currentWaypoint = 0;
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

        Vector3 dest = path.vectorPath[currentWaypoint];
        if(dest.x < transform.position.x){
            //rigidbody.velocity = new Vector3(-moveSpeed, rigidbody.velocity.y, 0f);
            transform.Translate(Vector3.right * -moveSpeed * Time.deltaTime);
        }
        else if(dest.x > transform.position.x){
            //rigidbody.velocity = new Vector3(moveSpeed, rigidbody.velocity.y, 0f);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
