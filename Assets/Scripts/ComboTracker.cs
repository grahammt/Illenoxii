using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTracker : MonoBehaviour
{
    public Text comboText;
    public float duration = 3.0f;
    public int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetComboText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetComboText(){
        comboText.text = "Combo x" + count.ToString()+"!";
    }

    public IEnumerator Timer(){
        Debug.Log("Start Timer");
        yield return new WaitForSeconds(duration);
        count = 0;
        SetComboText();
    }
}
