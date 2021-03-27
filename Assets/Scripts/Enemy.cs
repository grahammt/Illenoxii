using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType type;
    HasHealth healthScript;
    HasArmor armorScript;

    void Start()
    {
        if(type.contains.HasFlag(EnemyType.attributes.hasHealth)){
            healthScript = GetComponent<HasHealth>();
        }
        else if(type.contains.HasFlag(EnemyType.attributes.hasArmor)){
            armorScript = GetComponent<HasArmor>();
        }
    }
}
