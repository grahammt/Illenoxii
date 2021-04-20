using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimFire : MonoBehaviour
{
    public GameObject prefab;
    public GameObject arrow;
    public float maxCooldown = 5;
    public float detectionRadius = 10;
    public AudioClip firingSound;
    public Coroutine timer;
    private GameObject player;
    private bool inRange;
    public float cooldown;
    private bool firing;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!PausedGameManager.is_paused) {
            Vector3 diff = (player.transform.position - transform.position);
            inRange = (diff.magnitude < detectionRadius)? true : false;
            if (inRange && !GetComponent<HasArmor>().isStunned()){
                if (cooldown>=maxCooldown && !firing){
                    timer = StartCoroutine(Fire(Vector3.Normalize(diff)));
                }
                else{
                    if (!GetComponent<HasArmor>().isStunned()){
                        if (GetComponent<FlyingController>() && !GetComponent<FlyingController>().stunned)
                            cooldown+=Time.deltaTime;
                        else if (!GetComponent<FlyingController>()){
                            cooldown += Time.deltaTime;
                        }
                    }
                }
            }
            else{
                cooldown = 0;
            }
        }
    }


    IEnumerator Fire(Vector3 direction){
        Vector3 normal = Vector3.Normalize(direction);
        GetComponent<Animator>().SetTrigger("Firing");
        if (GetComponent<FlyingController>()!= null){
            GetComponent<FlyingController>().firing = true;
            arrow.transform.position = transform.position + normal*.75f;
        }
        else{
            arrow.transform.position = transform.position + normal*.75f + new Vector3(0.0f,0.12f,0.0f);
        }
        arrow.transform.localEulerAngles = Vector3.forward * Vector3.SignedAngle(Vector3.right,direction,Vector3.forward);
        firing = true;
        arrow.SetActive(true);
        AudioSource.PlayClipAtPoint(firingSound, transform.position);
        yield return new WaitForSeconds((1));
        if (!GetComponent<HasArmor>().isStunned())
        {
            yield return new WaitForSeconds((1.0f / 3.0f));
            if (!GetComponent<HasArmor>().isStunned())
            {
                GameObject bullet = Instantiate(prefab);
                bullet.GetComponent<Rigidbody2D>().velocity = normal * 5;
                bullet.transform.position = transform.position;
                bullet.GetComponent<BreakOnImpact>().sender = gameObject;
                bullet.GetComponent<BreakOnImpact>().damage = 20;
                if (GetComponent<FlyingController>()!= null){
                    yield return new WaitForSeconds(0.1f);
                    GetComponent<FlyingController>().firing = false;
                }
            }
        }
        arrow.SetActive(false);
        firing = false;
        cooldown = 0;
    }
}
