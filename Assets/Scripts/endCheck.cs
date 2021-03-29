using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject En1;
    public GameObject En2;
    public GameObject En3;
    public GameObject En4;
    public GameObject En5;
    public GameObject gameWinText;
    private bool ending = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(!En1 && !En2 && !En3 && !En4 && !En5 && !ending)
        {
            ending = true;
            StartCoroutine(End());
        }
    }
    IEnumerator End()
    {

        yield return new WaitForSeconds(0.4f);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameWinText.SetActive(true);
        //ending = false;
    }
}
