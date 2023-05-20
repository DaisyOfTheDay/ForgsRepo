using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("----- Spawner Settings -----")]
    [SerializeField] GameObject enemyTypeSpawned;
    [SerializeField] Transform[] spawnLoacations;
    [SerializeField] int numberToSpawn;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] bool spawnsOnLevelLoad;

    int currentNumberSpawned;
    public bool playerInRange;
    bool isSpawning;
    void Update()
    {
        if ((playerInRange || spawnsOnLevelLoad) && !isSpawning && currentNumberSpawned < numberToSpawn)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        isSpawning = true;

        if (spawnLoacations != null)
        {
            if(spawnLoacations.Length > 1)
            {
                SpawnEnemy(spawnLoacations[Random.Range(0, spawnLoacations.Length)]);
            }
            else
            {
                SpawnEnemy(spawnLoacations[0]);
            }
        }
        else
        {
            SpawnEnemy(this.gameObject.transform);
        }

        yield return new WaitForSeconds(timeBetweenSpawns);

        isSpawning = false;
    }

    public void SpawnEnemy(Transform locationToSpawn)
    {
        Instantiate(enemyTypeSpawned, locationToSpawn.position, locationToSpawn.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
}
