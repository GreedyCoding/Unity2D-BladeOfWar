using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Box")]
    [SerializeField] Collider2D spawnBox;

    [SerializeField] GameObject bossPrefab;
    private bool butterflyBossSpawned = false;

    private float nextTimeToSpawn = 0f;

    private float initialSpawnCooldown = 3f;
    private float spawnCooldown;

    private float spawnCooldownReduction = 0.1f;
    private float spawnCooldownDecreaseInterval = 20f;

    private void Start()
    {
        spawnCooldown = initialSpawnCooldown;
        StartCoroutine(DecreaseSpawnCooldown());
    }

    private void FixedUpdate()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if(nextTimeToSpawn <= Time.timeSinceLevelLoad)
        {
            nextTimeToSpawn = Time.timeSinceLevelLoad + spawnCooldown;

            if (Time.timeSinceLevelLoad < 60f)
            {
                GameObject eyePoolObject = ObjectPoolBeetleEnemies.SharedInstance.GetPooledObject();
                eyePoolObject.transform.position = GetRandomSpawnPosition();
                eyePoolObject.SetActive(true);
            }
            else if(Time.timeSinceLevelLoad < 120f)
            {
                GameObject ufoPoolObject = ObjectPoolDragonflyEnemies.SharedInstance.GetPooledObject();
                ufoPoolObject.transform.position = GetRandomSpawnPosition();
                ufoPoolObject.SetActive(true);
                if(Random.Range(0f, 1f) <= 0.5f)
                {
                    GameObject eyePoolObject = ObjectPoolBeetleEnemies.SharedInstance.GetPooledObject();
                    eyePoolObject.transform.position = GetRandomSpawnPosition();
                    eyePoolObject.SetActive(true);
                }
            }
            else if(Time.timeSinceLevelLoad < 180f)
            {
                GameObject eyePoolObject = ObjectPoolBeetleEnemies.SharedInstance.GetPooledObject();
                eyePoolObject.transform.position = GetRandomSpawnPosition();
                eyePoolObject.SetActive(true);
                GameObject ufoPoolObject = ObjectPoolDragonflyEnemies.SharedInstance.GetPooledObject();
                ufoPoolObject.transform.position = GetRandomSpawnPosition();
                ufoPoolObject.SetActive(true);
            }
            else if(Time.timeSinceLevelLoad > 180f)
            {
                if (!butterflyBossSpawned)
                {
                    Instantiate(bossPrefab, GetRandomSpawnPosition(), Quaternion.identity);
                    butterflyBossSpawned = true;
                }
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

    private IEnumerator DecreaseSpawnCooldown()
    {
        yield return new WaitForSeconds(spawnCooldownDecreaseInterval);
        spawnCooldown -= spawnCooldownReduction;
        StartCoroutine(DecreaseSpawnCooldown());
    }
}
