using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float speed;
    public float delay;
    public float damage;

    void Start()
    {
        StartCoroutine("DestroyGameObjectAfterDelay", delay);
        TextMeshPro text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();
    }

    void Update()
    {
        if(!PausedGameManager.is_paused) {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }

    IEnumerator DestroyGameObjectAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
