using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetWithFrame : MonoBehaviour
{
    public Transform target;
    public Vector3 frame;
    public Vector3 frameOffset;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target.position.x < transform.position.x + frameOffset.x - frame.x){
            transform.Translate(Vector3.right * (target.position.x - (transform.position.x + frameOffset.x - frame.x)));
        }
        else if(target.position.x > transform.position.x + frameOffset.x + frame.x){
            transform.Translate(Vector3.right * (target.position.x - (transform.position.x + frameOffset.x + frame.x)));
        }
        else if(target.position.y < transform.position.y + frameOffset.y - frame.y){
            transform.Translate(Vector3.up * (target.position.y - (transform.position.y + frameOffset.y - frame.y)));
        }
        else if(target.position.y > transform.position.y + frameOffset.y + frame.y){
            transform.Translate(Vector3.up * (target.position.y - (transform.position.y + frameOffset.y + frame.y)));
        }
    }
}
