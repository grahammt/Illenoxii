using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallBehavior : MonoBehaviour
{
    public float speed;
    public float lifetime = 10f;

    void Start()
    {
        StartCoroutine("DieAfterDelay", lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    IEnumerator DieAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
