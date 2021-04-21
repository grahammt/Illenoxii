using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class finalEndCheck : MonoBehaviour
{
    Subscription<EnemySpawnEvent> enemySpawnSubscription;
    Subscription<EnemyDieEvent> enemyDieSubscription;
    // Start is called before the first frame update
    private bool ending = true;
    private int enemyCount = 0;
    public GameObject FadeObj;

    void Start()
    {
        enemySpawnSubscription = EventBus.Subscribe<EnemySpawnEvent>(onEnemySpawn);
        enemyDieSubscription = EventBus.Subscribe<EnemyDieEvent>(onEnemyDie);
    }

    void Update()
    {
        if(!PausedGameManager.is_paused) {
            if(enemyCount == 0 && !ending){
                ending = true;
                FadeObj.GetComponent<Image>().color = Color.white;
                StartCoroutine(LoadSceneAfterDelay(SceneManager.GetActiveScene().buildIndex + 1));
            }
            /*if(!En1 && !En2 && !En3 && !En4 && !En5 && !En6 && !En7 && !En8 && !En9 && !En10 && !En11 && !En12 && !En18 && !En13 && !En14 && !En15 && !En16 && !En17 && !ending)
            {
                ending = true;
                StartCoroutine(End());
            }*/
            /*if(!En1 && !En2 && !En3 && !En4 && !En5 && !En6 && !En7 && !En8 && !ending)
            {
                ending = true;
                StartCoroutine(End());
            }*/
        }
    }

    IEnumerator LoadSceneAfterDelay(int sceneIndex){
        ComponentBank.instance.SceneTransitionAnimator.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
    }

    void onEnemySpawn(EnemySpawnEvent e){
        enemyCount += 1;
        ending = false;
    }

    void onEnemyDie(EnemyDieEvent e){
        enemyCount -= 1;
    }

    void OnDestroy(){
        EventBus.Unsubscribe(enemySpawnSubscription);
        EventBus.Unsubscribe(enemyDieSubscription);
    }
}
