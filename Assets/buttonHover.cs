using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonHover : MonoBehaviour
{
    public bool enlarged;
    public void enlarge()
    {
        GetComponent<RectTransform>().localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }
    public void shrink()
    {
        GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
