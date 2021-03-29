using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public TextMeshProUGUI text;

    public void SetMaxHealth(float max_hp)
    {
        slider.maxValue = max_hp;
        slider.value = max_hp;
    }

    public void SetCurrHealth(float curr_hp)
    {
        slider.value = curr_hp;
        if(text){
            text.text = "" + curr_hp + "/" + slider.maxValue;
        }
    }
}
