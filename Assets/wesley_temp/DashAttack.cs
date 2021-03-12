using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{

    public Animator dashAnim;
    public Transform dashAnimPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("e")){
            dashAnimPos.position.Set(-0.8f, -0.8f, 0f);
            dashAnim.SetTrigger("Dash");
            transform.Translate(new Vector3(2f, 2f, 0f));
        }
    }
}
