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
    private bool firing;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(!PausedGameManager.is_paused) {
            Vector3 diff = (player.transform.position - transform.position);
            inRange = (diff.magnitude < detectionRadius)? true : false;
            if (inRange && !GetComponent<HasArmor>().isStunned()){
                if (cooldown>=maxCooldown && !firing){
                    StartCoroutine(Fire(Vector3.Normalize(diff)));
                }
                else{
                    cooldown+=Time.deltaTime;
                }
            }
            else{
                cooldown = 0;
            }
        }
    }


    IEnumerator Fire(Vector3 direction){
        GetComponent<Animator>().SetTrigger("Firing");
        firing = true;
        yield return new WaitForSeconds((1));
        if (!GetComponent<HasArmor>().isStunned())
        {
            yield return new WaitForSeconds((1.0f / 3.0f));
            firing = false;
            cooldown = 0;
            if (!GetComponent<HasArmor>().isStunned())
            {
                GameObject bullet = Instantiate(prefab);
                bullet.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(direction) * 5;
                bullet.transform.position = transform.position;
                bullet.GetComponent<BreakOnImpact>().sender = gameObject;
                bullet.GetComponent<BreakOnImpact>().damage = 20;
            }
        }
        firing = false;
        cooldown = 0;

    }
}
