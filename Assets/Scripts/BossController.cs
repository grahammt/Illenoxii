using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    private float enemy_spawn_timer_MAX = 15f;
    private float enemy_spawn_timer = 0.0f;
    private float projectile_spawn_timer_MAX = 20f;
    private float projectile_spawn_timer = 0.0f;
    public float enemy_wave_count = 3;
    public float fire_prefab_count = 7;
    public GameObject enemyPrefab;
    public GameObject firePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!PausedGameManager.is_paused) {
            // Adjust enemy spawn timer, spawn new enemies if time has passed
            enemy_spawn_timer += Time.deltaTime;
            if (enemy_spawn_timer >= enemy_spawn_timer_MAX) {
                enemy_spawn_timer = 0;
                SpawnNewEnemies();
            }

            // Adjust firewall spawn timer, spawn new if time has passed
            projectile_spawn_timer += Time.deltaTime;
            if (projectile_spawn_timer >= projectile_spawn_timer_MAX) {
                projectile_spawn_timer = 0;
                SpawnFireWall();
            }
        }
    }

    void SpawnNewEnemies() {
        Vector3 spawn_offset = Vector3.left * 10;
        Vector3 enemy_offset = Vector3.left * 5;

        for(int i = 1; i <= enemy_wave_count; ++i) {
            Instantiate(enemyPrefab, transform.position + spawn_offset + enemy_offset * i, Quaternion.identity);
        }
    }

    void SpawnFireWall() {
        // Precalculate spawn values
        // GameObject [] fireWall;
        Vector3 first_spawn_offset = new Vector3 (-10f, -20f, 0);
        Vector3 vertical_offset = Vector3.up * -10f;
        Vector3 first_spawn = transform.position + first_spawn_offset;

        // There are 2 types of fire-walls that we spawn: one where you
        // can manuever through by jumping, and another you have to dash
        // through.
        int type_wall = Random.Range(0, 2);

        if(type_wall == 1) {
            // Spawn Dash only wall
            for(int i = 0; i < fire_prefab_count; ++i) {
                Instantiate(firePrefab, first_spawn + vertical_offset * i, Quaternion.identity);
            }
        } else {
            // Spawn Jump through wall
            // We will spawn the normal wall but without 3 of the fireballs
            // Find the middle ball to not spawn (in range 2-4)
            int notSpawnMiddle = Random.Range(2, 5);

            for(int i = 0; i < fire_prefab_count; ++i) {
                // check if we want to spawn this one
                if(!(Mathf.Abs(notSpawnMiddle - i) < 2)) {
                    Instantiate(firePrefab, first_spawn + vertical_offset * i, Quaternion.identity);
                }
            }
        }

    }
}