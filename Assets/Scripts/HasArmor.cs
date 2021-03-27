using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasArmor : MonoBehaviour
{
    public int armorPoints;
    int ap;

    void Start(){
        ap = armorPoints;
    }

    public void LoseArmor(int points){
        ap -= points;
    }

    public void ResetArmor(){
        ap = armorPoints;
    }
}
