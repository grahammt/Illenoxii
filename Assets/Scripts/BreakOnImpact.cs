using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnImpact : MonoBehaviour
{
    public GameObject player;
    public IEnumerator timer = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<HasHealth>().takeDamage(25);
            player.GetComponent<InputToPlayerAttacks>().SecondaryHit();
        }
        if (other.gameObject.tag != "Player"){
            Destroy(gameObject);
        }
    }
}
