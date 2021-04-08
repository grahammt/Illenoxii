using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathGenerator : MonoBehaviour
{
    public Transform target;
    [HideInInspector]
    public Path path;
    Seeker seeker;

    void Start(){
        seeker = GetComponent<Seeker>();
        InvokeRepeating("RecalculatePath", 1f, 0.5f);
    }

    void RecalculatePath(){
        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    void OnPathComplete (Path p) {
        if (!p.error) {
            path = p;
        }
    }
}
