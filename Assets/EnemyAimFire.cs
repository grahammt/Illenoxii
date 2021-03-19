using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimFire : MonoBehaviour
{
    public GameObject prefab;
    public float maxCooldown = 5;
    public float detectionRadius = 10;
    private GameObject player;
    private bool inRange;
    private float cooldown;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = (player.transform.position - transform.position);
        inRange = (diff.magnitude < detectionRadius)? true : false;
        if (inRange){
            if (cooldown>=maxCooldown){
                Fire(Vector3.Normalize(diff));
                cooldown = 0;
            }
            else{
                cooldown+=Time.deltaTime;
            }
        }
        else{
            cooldown = 0;
        }
    }

    void Fire(Vector3 direction){
        GameObject bullet = Instantiate(prefab);
        bullet.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(direction) * 5;
        bullet.transform.position = transform.position;
        Debug.Log("Fire!");
    }
}
