using System;
using UnityEngine;

public class EnemyLootDropController : MonoBehaviour
{
    public static EnemyLootDropController Instance;

    [SerializeField] GameObject _bonusDropPrefab;
    [SerializeField] GameObject _malusDropPrefab;
    [SerializeField] GameObject _coinDropPrefab;

    private float _bonusDropChance = 0.33f;
    private float _coinDropChance = 0.33f;
    private float _malusDropChance = 0.33f;

    private float _bonusDropRange;
    private float _coinDropRange;
    private float _malusDropRange;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _bonusDropRange = _bonusDropChance;
        _coinDropRange = _coinDropChance + _bonusDropChance;
        _malusDropRange = _malusDropChance + _bonusDropChance + _coinDropChance;
    }

    public void DropLoot(Vector3 dropLocation)
    {
        float randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber <= _bonusDropRange)
        {
            Instantiate(_bonusDropPrefab, dropLocation, Quaternion.identity);
            return;
        }
        else if (randomNumber > _bonusDropRange && randomNumber <= _coinDropRange)
        {
            Instantiate(_coinDropPrefab, dropLocation, Quaternion.identity);
            return;
        }
        else if (randomNumber > _coinDropChance && randomNumber <= _malusDropRange)
        {
            Instantiate(_malusDropPrefab, dropLocation, Quaternion.identity);
            return;
        }
    }
}
