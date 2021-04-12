using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyDriver : MonoBehaviour
{
    Enemy enemyScript;

    void Start()
    {
        enemyScript = GetComponent<Enemy>();
        enemyScript.deathCallBack += DeathHandler;
    }

    public void DeathHandler(){
        Destroy(gameObject);
    }
}
