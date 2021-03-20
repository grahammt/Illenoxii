using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnImpact : MonoBehaviour
{
    public GameObject sender;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject != sender){
            if (other.gameObject.GetComponent<HasHealth>()!=null){
                other.gameObject.GetComponent<HasHealth>().takeDamage(damage);
                //player.GetComponent<InputToPlayerAttacks>().SecondaryHit();
            }
            if (other.gameObject.tag != sender.tag){
                Destroy(gameObject);
            }
        }
    }
}
