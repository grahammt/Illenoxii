using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMovement : MonoBehaviour
{
    public Transform target;
    public Vector2 scale;
    Vector3 prev_pos;

    void Start()
    {
        prev_pos = target.position;
    }

    void Update()
    {
        if(!PausedGameManager.is_paused) {
            transform.Translate(new Vector3((target.position.x - prev_pos.x) * scale.x, (target.position.y - prev_pos.y) * scale.y, 0f));
            prev_pos = target.position;
        }
    }
}
