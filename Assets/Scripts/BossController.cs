using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float enemy_spawn_timer_MAX = 20f;
    private float enemy_spawn_timer = 10.0f;
    public float projectile_spawn_timer_MAX = 15f;
    private float projectile_spawn_timer = 0.0f;
    public float enemy_wave_count = 3;
    public float fire_prefab_count = 10;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject flyingPrefab;
    public GameObject firePrefab;
    public GameObject firewallPrefab;
    private Transform playerTransform;
    public float movement_timer_MAX = 3f;
    private float movement_timer = 0.0f;
    public float speed = 2;
    public AudioClip spawn_sound;
    Enemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        SpawnFireWallV2();
        playerTransform = GameObject.Find("Player").transform;
        GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
        enemyScript = GetComponentInChildren<Enemy>();

        enemyScript.deathCallBack += DeathHandler;
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
                SpawnFireWallV2();
            }

            // Adjust movement
            movement_timer += Time.deltaTime;
            if (movement_timer >= movement_timer_MAX) {
                movement_timer = 0;
                GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * -1;
            }
        }
    }

    void SpawnNewEnemies() {
        Vector3 spawn_offset = Vector3.left * 0;
        Vector3 enemy_offset = Vector3.left * 3;
        Vector3 flying_offset = Vector3.up;

        // Spawn flame enemy
        AudioSource.PlayClipAtPoint(spawn_sound, transform.position);
        for(int i = 1; i <= enemy_wave_count; ++i) {
            if (i == 1) {
                GameObject temp1 = Instantiate(enemyPrefab2, transform.position + spawn_offset + enemy_offset * i, Quaternion.identity);
                temp1.GetComponent<platformerPathfinding>().target = playerTransform;
            } else {
                GameObject temp2 = Instantiate(enemyPrefab1, transform.position + spawn_offset + enemy_offset * i, Quaternion.identity);
                temp2.GetComponent<platformerPathfinding>().target = playerTransform;
            }
        }

        // Spawn air enemy
        GameObject temp = Instantiate(flyingPrefab, transform.position + enemy_offset + flying_offset, Quaternion.identity);
        temp.GetComponent<platformerPathfinding>().target = playerTransform;
    }

    void SpawnFireWall() {
        // Precalculate spawn values
        // GameObject [] fireWall;
        Vector3 first_spawn_offset = new Vector3 (20f, -5f, 0);
        Vector3 vertical_offset = Vector3.up * 1f;
        Vector3 first_spawn = transform.position + first_spawn_offset;

        // There are 2 types of fire-walls that we spawn: one where you
        // can manuever through by jumping, and another you have to dash
        // through.
        int type_wall = Random.Range(0, 2);

        if(false && type_wall == 1) {
            // Spawn Dash only wall
            for(int i = 0; i < fire_prefab_count; ++i) {
                GameObject temp = Instantiate(firePrefab, first_spawn + vertical_offset * i, Quaternion.identity);
                temp.GetComponent<BreakOnImpact>().sender = gameObject;
                temp.GetComponent<BreakOnImpact>().damage = 20;
                temp.GetComponent<Rigidbody2D>().velocity = Vector3.left * 5;
            }
        } else {
            // Spawn Jump through wall
            // We will spawn the normal wall but without 3 of the fireballs
            // Find the middle ball to not spawn (in range 2-4)
            int notSpawnMiddle = Random.Range(2, 5);

            for(int i = 0; i < fire_prefab_count; ++i) {
                // check if we want to spawn this one
                if(!(Mathf.Abs(notSpawnMiddle - i) < 2)) {
                    GameObject temp = Instantiate(firePrefab, first_spawn + vertical_offset * i, Quaternion.identity);
                    temp.GetComponent<BreakOnImpact>().sender = gameObject;
                    temp.GetComponent<BreakOnImpact>().damage = 20;
                    temp.GetComponent<Rigidbody2D>().velocity = Vector3.left * 5;
                }
            }
        }

    }

    void SpawnFireWallV2(){
        Vector3 firewallSpawn = new Vector3(20f, -1f, 0f);
        GameObject firewall = GameObject.Instantiate(firewallPrefab, firewallSpawn, Quaternion.identity);
        firewall.GetComponent<FirewallBehavior>().speed = -3f;
    }

    void DeathHandler(){
        Destroy(gameObject);
    }
}
