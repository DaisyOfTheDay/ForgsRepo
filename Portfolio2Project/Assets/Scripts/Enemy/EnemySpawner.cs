using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [Header("----- Spawner Settings -----")]
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] int baseNumberToSpawn;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] bool spawnsOnLevelLoad;

    [Header("----- Set By Collider -----")]
    public bool playerDetected;

    int currentNumberSpawned;
    int totalToSpawn;
    bool isSpawning;
    LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.instance;
        totalToSpawn = levelManager.currentLevel + baseNumberToSpawn - 1;
        currentNumberSpawned = 0;
    }
    void Update()
    {
        if ((playerDetected || spawnsOnLevelLoad) && isSpawning == false && currentNumberSpawned < totalToSpawn)
        {
            StartCoroutine(SpawnEnemies());
            Debug.Log("Spawning Enemies");
        }
    }

    IEnumerator SpawnEnemies()
    {
        isSpawning = true;

        if(spawnPositions.Length > 0)
        {
            if (spawnPositions.Length > 1)
            {
                SpawnEnemy(spawnPositions[Random.Range(0, spawnPositions.Length)]);
                Debug.Log("Enemy Spawned at Spawn random");
            }
            else
            {
                SpawnEnemy(spawnPositions[0]);
                Debug.Log("Enemy Spawned at Spawn 0");
            }
        }
        else
        {
            SpawnEnemy(this.gameObject.transform);
            Debug.Log("Enemy Spawned Locally");
        }

        yield return new WaitForSeconds(timeBetweenSpawns);

        isSpawning = false;
    }

    public void SpawnEnemy(Transform locationToSpawn)
    {
        Vector3 randomPosition = new Vector3(locationToSpawn.position.x + (Random.Range(0, 3)), locationToSpawn.position.y, locationToSpawn.position.z + (Random.Range(0, 3)));
        Instantiate(enemyToSpawn, randomPosition, locationToSpawn.rotation);
        ++currentNumberSpawned;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            Debug.Log("Player Detected");
        }
    }
}
