﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endCheck : MonoBehaviour
{
    Subscription<EnemySpawnEvent> enemySpawnSubscription;
    Subscription<EnemyDieEvent> enemyDieSubscription;
    // Start is called before the first frame update
    /*public GameObject En1;
    public GameObject En2;
    public GameObject En3;
    public GameObject En4;
    public GameObject En5;
    public GameObject En6;
    public GameObject En7;
    public GameObject En8;*/
    public GameObject En8;
    public GameObject En9;
    public GameObject En10;
    public GameObject En11;
    public GameObject En12;
    public GameObject En13;
    public GameObject En14;
    public GameObject En15;
    public GameObject En16;
    public GameObject En17;
    public GameObject En18;
    public GameObject gameWinText;
    private bool ending = false;
    public int enemyCount = 0;

    void Start()
    {
        enemySpawnSubscription = EventBus.Subscribe<EnemySpawnEvent>(onEnemySpawn);
        enemyDieSubscription = EventBus.Subscribe<EnemyDieEvent>(onEnemyDie);
    }

    void Update()
    {
        if(!PausedGameManager.is_paused) {
            if(enemyCount == 0 && !ending){
            if(!En1 && !En2 && !En3 && !En4 && !En5 && !En6 && !En7 && !En8 && !En9 && !En10 && !En11 && !En12 && !En18 && !En13 && !En14 && !En15 && !En16 && !En17 && !ending)
            {
                ending = true;
                StartCoroutine(End());
            }
            /*if(!En1 && !En2 && !En3 && !En4 && !En5 && !En6 && !En7 && !En8 && !ending)
            {
                ending = true;
                StartCoroutine(End());
            }*/
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
    }

    void onEnemyDie(EnemyDieEvent e){
        enemyCount -= 1;
    }

    void OnDestroy(){
        EventBus.Unsubscribe(enemySpawnSubscription);
        EventBus.Unsubscribe(enemyDieSubscription);
    }
}
