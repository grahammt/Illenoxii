using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallBehavior : MonoBehaviour
{
    public float speed;
    public float lifetime = 10f;
    private bool spawned = false;

    void Start()
    {
        StartCoroutine("DieAfterDelay", lifetime);
    }

    void Update()
    {
        if (!spawned){
            EventBus.Publish<EnemySpawnEvent>(new EnemySpawnEvent());
            spawned = true;
        }
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    IEnumerator DieAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        EventBus.Publish<EnemyDieEvent>(new EnemyDieEvent());
        Destroy(gameObject);
    }
}
