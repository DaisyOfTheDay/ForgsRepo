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
    // Update is called once per frame
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
            Transform spawnLocation = spawnLoacations[Random.Range(0, spawnLoacations.Length)];
            SpawnEnemy(spawnLocation);
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
