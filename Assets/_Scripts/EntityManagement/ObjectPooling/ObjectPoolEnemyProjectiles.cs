using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolEnemyProjectiles : MonoBehaviour
{
    //Singleton
    public static ObjectPoolEnemyProjectiles SharedInstance;

    //List of pooled objects
    [SerializeField] List<GameObject> _pooledObjects;

    //Objects to pool
    [SerializeField] GameObject _defaultObjectToPool;
    [SerializeField] GameObject _stageTwoObjectToPool;
    [SerializeField] int _amountToPool;

    [SerializeField] IntEventChannelSO _stageChangedIntEventChannel;

    void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
    }

    private void OnEnable()
    {
        _stageChangedIntEventChannel.OnEventRaised += SetupNewPool;
    }

    private void OnDisable()
    {
        _stageChangedIntEventChannel.OnEventRaised -= SetupNewPool;
    }

    void Start()
    {
        SetupPool();
    }

    void SetupPool()
    {
        _pooledObjects = new List<GameObject>();
        GameObject tempPoolItem;
        for (int i = 0; i < _amountToPool; i++)
        {
            tempPoolItem = Instantiate(_defaultObjectToPool);
            tempPoolItem.SetActive(false);
            _pooledObjects.Add(tempPoolItem);
        }
    }

    void SetupNewPool(int stage)
    {
        if (_pooledObjects.Count > 0)
        {
            foreach (GameObject pooledObject in _pooledObjects)
            {
                Destroy(pooledObject);
            }
        }

        _pooledObjects = new List<GameObject>();
        GameObject gameObject;

        switch (stage)
        {
            case 0:
                gameObject = null;
                Debug.LogError("Stage 0 does not exist. Can´t setup pool.");
                break;
            case 1:
                gameObject = null;
                Debug.LogError("Stage 1 is automatically setup. Can´t setup pool.");
                break;
            case 2:
                gameObject = _stageTwoObjectToPool;
                break;
            default:
                gameObject = null;
                Debug.LogError("Unknown stage number detected. Can´t setup pool.");
                break;
        }

        if (gameObject == null) return;

        GameObject tempPoolItem;
        for (int i = 0; i < _amountToPool; i++)
        {
            tempPoolItem = Instantiate(_stageTwoObjectToPool);
            tempPoolItem.SetActive(false);
            _pooledObjects.Add(tempPoolItem);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }
        return null;
    }
}
