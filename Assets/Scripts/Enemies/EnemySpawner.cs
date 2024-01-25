using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Box")]
    [SerializeField] Collider2D spawnBox;

    private float nextTimeToSpawn = 0f;

    private float initialSpawnCooldown = 3f;
    private float spawnCooldown;

    private float spawnCooldownReduction = 0.1f;
    private float spawnCooldownDecreaseInterval = 10f;

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
        if (Time.time % spawnCooldownDecreaseInterval == 0)
        {
            spawnCooldown -= spawnCooldownReduction;
        }

        if(nextTimeToSpawn <= Time.time)
        {
            nextTimeToSpawn = Time.time + spawnCooldown;
            if (Time.time < 60f)
            {
                GameObject poolObject = ObjectPoolEyeEnemies.SharedInstance.GetPooledObject();
                poolObject.transform.position = GetRandomSpawnPosition();
                poolObject.SetActive(true);
            }
            else
            {
                GameObject poolObject = ObjectPoolUfoEnemies.SharedInstance.GetPooledObject();
                poolObject.transform.position = GetRandomSpawnPosition();
                poolObject.SetActive(true);
            }
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
