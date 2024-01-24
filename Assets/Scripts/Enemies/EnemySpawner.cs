using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] GameObject eyeEnemyPrefab;
    [SerializeField] GameObject ufoEnemyPrefab;

    [Header("Spawn Box")]
    [SerializeField] Collider2D spawnBox;

    float nextTimeToSpawn = 0f;

    float initialSpawnCooldown = 3f;
    float spawnCooldown;

    float spawnCooldownReduction = 0.1f;
    float spawnCooldownDecreaseInterval = 10f;

    private void Start()
    {
        spawnCooldown = initialSpawnCooldown;
    }

    private void FixedUpdate()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    { 
        GameObject enemyToSpawn;

        if (Time.time % spawnCooldownDecreaseInterval == 0)
        {
            spawnCooldown -= spawnCooldownReduction;
        }

        if (Time.time < 60f)
        {
            enemyToSpawn = eyeEnemyPrefab;
        }
        else
        {
            enemyToSpawn = ufoEnemyPrefab;
        }

        if(nextTimeToSpawn <= Time.time)
        {
            nextTimeToSpawn = Time.time + spawnCooldown;
            Instantiate(enemyToSpawn, GetRandomSpawnPosition(), Quaternion.identity);
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        var bounds = spawnBox.bounds;
        var randomX = Random.Range(bounds.min.x, bounds.max.x);
        var randomY = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(randomX, randomY);
    }
}
