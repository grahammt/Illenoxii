using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : MonoBehaviour
{
    public bool stunned;
    public bool firing;
    public float speed;
    public float duration = 3.0f;
    private float leftPoint;
    private float rightPoint;
    private Rigidbody2D rigidbody;
    private string direction;
    private float altitude;
    private float clock;

    // Start is called before the first frame update
    void Start()
    {
        altitude = transform.position.y;
        rigidbody = GetComponent<Rigidbody2D>();
        leftPoint = transform.position.x - 5;
        rightPoint = transform.position.x + 5;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int left = 0;
        if (direction == "left"){
            left = 1;
        }
        else{
            left = 0;
        }
        if (stunned){
            if (onGround()){
                if (clock<duration){
                    clock+=Time.deltaTime;
                }
                else{
                    clock = 0;
                    StartCoroutine(FlyBackUp());
                }
            }
        }
        else{
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            if (firing){
                rigidbody.velocity = Vector2.zero;
            }
            else{
                rigidbody.velocity = new Vector2(speed * Mathf.Pow(-1,left),0);
                if (!inRange()){
                    turnAround();
                }
            }
        }
    }
    bool onGround(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Terrain"));
        Debug.DrawRay(transform.position, Vector3.down * 1.1f);
        return hit.collider != null;
    }
    IEnumerator FlyBackUp(){
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        while (transform.position.y < altitude){
            rigidbody.velocity = new Vector2(0.0f, speed/1.125f);
            yield return null;
        }
        if (GetComponent<HasArmor>()){
            GetComponent<HasArmor>().currentStun = 0;
        }
        transform.position = new Vector2(transform.position.x,altitude);
        stunned = false;
    }

    bool inRange(){
        if (transform.position.x < rightPoint && transform.position.x > leftPoint){
            return true;
        }
        else{
            return false;
        }
    }

    void turnAround(){
        if (direction=="left"){
            direction = "right";
            rigidbody.velocity = new Vector2(speed,0.0f);
        }
        else{
            direction = "left";
            rigidbody.velocity = new Vector2(-speed,0.0f);
        }
    }
}
