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
    private float spawnCooldownDecreaseInterval = 30f;

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

            if (Time.timeSinceLevelLoad < 10f)
            {
                GameObject beetlePoolObject = ObjectPoolBeetleEnemies.SharedInstance.GetPooledObject();
                beetlePoolObject.transform.position = GetRandomSpawnPosition();
                beetlePoolObject.SetActive(true);
            }
            else if (Time.timeSinceLevelLoad < 20f)
            {
                GameObject dragonflyPoolObject = ObjectPoolDragonflyEnemies.SharedInstance.GetPooledObject();
                dragonflyPoolObject.transform.position = GetRandomSpawnPosition();
                dragonflyPoolObject.SetActive(true);
                if (Random.Range(0f, 1f) <= 0.5f)
                {
                    GameObject beetlePoolObject = ObjectPoolBeetleEnemies.SharedInstance.GetPooledObject();
                    beetlePoolObject.transform.position = GetRandomSpawnPosition();
                    beetlePoolObject.SetActive(true);
                }
            }
            else if (Time.timeSinceLevelLoad < 30f)
            {
                GameObject dragonflyPoolObject = ObjectPoolDragonflyEnemies.SharedInstance.GetPooledObject();
                dragonflyPoolObject.transform.position = GetRandomSpawnPosition();
                dragonflyPoolObject.SetActive(true);
                if (Random.Range(0f, 1f) <= 0.75f)
                {
                    GameObject beetlePoolObject = ObjectPoolBeetleEnemies.SharedInstance.GetPooledObject();
                    beetlePoolObject.transform.position = GetRandomSpawnPosition();
                    beetlePoolObject.SetActive(true);
                }
            }
            else if (Time.timeSinceLevelLoad > 30f)
            {
                if (!butterflyBossSpawned)
                {
                    MessagePopupController.Instance.PlayMessage("Boss Incoming!");
                    butterflyBossSpawned = true;
                    StartCoroutine(SpawnButterflyBoss());
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

    private IEnumerator SpawnButterflyBoss()
    {
        yield return new WaitForSeconds(3f);
        GameObject boss = Instantiate(bossPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        boss.SetActive(true);
    }
}
