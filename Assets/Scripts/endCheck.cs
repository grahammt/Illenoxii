using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endCheck : MonoBehaviour
{
    Subscription<EnemySpawnEvent> enemySpawnSubscription;
    Subscription<EnemyDieEvent> enemyDieSubscription;
    Subscription<PlayerDieEvent> playerDeathSub;
    // Start is called before the first frame update
    public GameObject gameWinText;
    private bool ending = true;
    private int enemyCount = 0;
    private bool lost = false;
    Transform player;
    Animator playerAnim;
    public Animator playerLowerAnim;
    Rigidbody2D playerRb;

    void Start()
    {
        enemySpawnSubscription = EventBus.Subscribe<EnemySpawnEvent>(onEnemySpawn);
        enemyDieSubscription = EventBus.Subscribe<EnemyDieEvent>(onEnemyDie);
        playerDeathSub = EventBus.Subscribe<PlayerDieEvent>(onPlayerDeath);
        player = ComponentBank.instance.playerTransform;
        playerRb = player.GetComponent<Rigidbody2D>();
        playerAnim = player.GetComponent<Animator>();
        //playerLowerAnim = player.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(!PausedGameManager.is_paused) {
            if(enemyCount == 0 && !ending && !lost){
                ending = true;
                StartCoroutine(End());
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

    void LateUpdate(){
        if(ending){
            playerAnim.SetBool("isRunning", false);
            playerLowerAnim.SetBool("isRunning", false);
            playerRb.velocity = new Vector3(0f, playerRb.velocity.y, 0f);
            player.GetComponent<PlayerMovement>().StopRunSound();
        }
    }

    IEnumerator End()
    {

        yield return new WaitForSeconds(0.4f);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameWinText.SetActive(true);
        //ending = false;
    }

    void onEnemySpawn(EnemySpawnEvent e){
        enemyCount += 1;
        ending = false;
    }

    void onEnemyDie(EnemyDieEvent e){
        enemyCount -= 1;
    }

    void onPlayerDeath(PlayerDieEvent e){
        lost = true;
    }

    void OnDestroy(){
        EventBus.Unsubscribe(enemySpawnSubscription);
        EventBus.Unsubscribe(enemyDieSubscription);
        EventBus.Unsubscribe(playerDeathSub);
    }
}
