using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Box")]
    [SerializeField] Collider2D _spawnBox;

    [Header("Boss Prefabs")]
    [SerializeField] GameObject _stageOneBossPrefab;
    [SerializeField] GameObject _stageTwoBossPrefab;

    [Header("Event Channels")]
    [SerializeField]VoidEventChannelSO _bossDeathEventChannel;
    [SerializeField]IntEventChannelSO _stageChangeIntEventChannel;

    private GameplayTimer _timer;

    private bool _bossSpawned = false;

    private float _nextTimeToSpawn = 0f;

    private float _initialSpawnCooldown = 3f;
    private float _spawnCooldown;

    private float _phaseOneTime = 60f;
    private float _phaseTwoTime = 120f;
    private float _phaseThreeTime = 180f;

    private float _spawnCooldownReduction = 0.1f;
    private float _spawnCooldownDecreaseInterval = 30f;

    private int _stage = 1;

    private void OnEnable()
    {
        _bossDeathEventChannel.OnEventRaised += IncreaseStage;
    }

    private void OnDisable()
    {
        _bossDeathEventChannel.OnEventRaised -= IncreaseStage;
    }

    private void Start()
    {
        _timer = GameplayTimer.Instance;
        _timer.StartTimer();

        _spawnCooldown = _initialSpawnCooldown;
        StartCoroutine(DecreaseSpawnCooldown());
    }

    private void IncreaseStage()
    {
        _stage++;
        _stageChangeIntEventChannel.RaiseEvent(_stage);
    }

    private void FixedUpdate()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if(_nextTimeToSpawn <= _timer.CurrentTime)
        {
            _nextTimeToSpawn = _timer.CurrentTime + _spawnCooldown;

            EvaluateAndSpawnEnemy();
            
        }
    }

    private void EvaluateAndSpawnEnemy()
    {
        if (_timer.CurrentTime < _phaseOneTime)
        {
            GameObject smallEnemyPoolObject = ObjectPoolSmallEnemies.SharedInstance.GetPooledObject();
            smallEnemyPoolObject.transform.position = GetRandomSpawnPosition();
            smallEnemyPoolObject.SetActive(true);
        }
        else if (_timer.CurrentTime < _phaseTwoTime)
        {
            GameObject bigEnemyPoolObject = ObjectPoolBigEnemies.SharedInstance.GetPooledObject();
            bigEnemyPoolObject.transform.position = GetRandomSpawnPosition();
            bigEnemyPoolObject.SetActive(true);
            if (Random.Range(0f, 1f) <= 0.5f)
            {
                GameObject smallEnemyPoolObject = ObjectPoolSmallEnemies.SharedInstance.GetPooledObject();
                smallEnemyPoolObject.transform.position = GetRandomSpawnPosition();
                smallEnemyPoolObject.SetActive(true);
            }
        }
        else if (_timer.CurrentTime < _phaseThreeTime)
        {
            GameObject bigEnemyPoolObject = ObjectPoolBigEnemies.SharedInstance.GetPooledObject();
            bigEnemyPoolObject.transform.position = GetRandomSpawnPosition();
            bigEnemyPoolObject.SetActive(true);
            if (Random.Range(0f, 1f) <= 0.75f)
            {
                GameObject smallEnemyPoolObject = ObjectPoolSmallEnemies.SharedInstance.GetPooledObject();
                smallEnemyPoolObject.transform.position = GetRandomSpawnPosition();
                smallEnemyPoolObject.SetActive(true);
            }
        }
        else if (_timer.CurrentTime > _phaseThreeTime)
        {
            if (!_bossSpawned)
            {
                MessagePopupController.Instance.PlayMessage("Boss Incoming!");
                _bossSpawned = true;

                if (_stage == 1)
                    StartCoroutine(SpawnBoss(_stageOneBossPrefab));

                if (_stage == 2)
                    StartCoroutine(SpawnBoss(_stageTwoBossPrefab));

                _timer.StopTimer();
                _timer.ResetTimer();
                _bossSpawned = false;
                _nextTimeToSpawn = 0f;
            }
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        var bounds = _spawnBox.bounds;
        var randomX = Random.Range(bounds.min.x, bounds.max.x);
        var randomY = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(randomX, randomY);
    }

    private IEnumerator DecreaseSpawnCooldown()
    {
        yield return new WaitForSeconds(_spawnCooldownDecreaseInterval);
        _spawnCooldown -= _spawnCooldownReduction;
        StartCoroutine(DecreaseSpawnCooldown());
    }

    private IEnumerator SpawnBoss(GameObject prefab)
    {
        yield return new WaitForSeconds(3f);
        GameObject boss = Instantiate(prefab, GetRandomSpawnPosition(), Quaternion.identity);
        boss.SetActive(true);
    }
}
