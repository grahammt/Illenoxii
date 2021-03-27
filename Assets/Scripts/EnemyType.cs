using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blank enemy", menuName = "Enemy Type")]
public class EnemyType : ScriptableObject
{
    [Flags]
    public enum attributes : short
    {
        nothing = 0,
        hasHealth = 1,
        hasKnockback = 2,
        hasArmor = 4
    };

    public attributes contains;
}
