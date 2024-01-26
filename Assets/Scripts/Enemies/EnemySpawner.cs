using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Box")]
    [SerializeField] Collider2D spawnBox;

    [SerializeField] GameObject bossPrefab;

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
        if (Time.timeSinceLevelLoad % spawnCooldownDecreaseInterval == 0)
        {
            spawnCooldown -= spawnCooldownReduction;
        }

        if(nextTimeToSpawn <= Time.timeSinceLevelLoad)
        {
            nextTimeToSpawn = Time.timeSinceLevelLoad + spawnCooldown;

            if (Time.timeSinceLevelLoad < 60f)
            {
                GameObject eyePoolObject = ObjectPoolEyeEnemies.SharedInstance.GetPooledObject();
                eyePoolObject.transform.position = GetRandomSpawnPosition();
                eyePoolObject.SetActive(true);
            }
            else if(Time.timeSinceLevelLoad < 120f)
            {
                GameObject ufoPoolObject = ObjectPoolUfoEnemies.SharedInstance.GetPooledObject();
                ufoPoolObject.transform.position = GetRandomSpawnPosition();
                ufoPoolObject.SetActive(true);
                if(Random.Range(0f, 1f) <= 0.5f)
                {
                    GameObject eyePoolObject = ObjectPoolEyeEnemies.SharedInstance.GetPooledObject();
                    eyePoolObject.transform.position = GetRandomSpawnPosition();
                    eyePoolObject.SetActive(true);
                }
            }
            else if(Time.timeSinceLevelLoad < 180f)
            {
                GameObject eyePoolObject = ObjectPoolEyeEnemies.SharedInstance.GetPooledObject();
                eyePoolObject.transform.position = GetRandomSpawnPosition();
                eyePoolObject.SetActive(true);
                GameObject ufoPoolObject = ObjectPoolUfoEnemies.SharedInstance.GetPooledObject();
                ufoPoolObject.transform.position = GetRandomSpawnPosition();
                ufoPoolObject.SetActive(true);
            }
            else if(Time.timeSinceLevelLoad > 180f)
            {
                Instantiate(bossPrefab, GetRandomSpawnPosition(), Quaternion.identity);
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
