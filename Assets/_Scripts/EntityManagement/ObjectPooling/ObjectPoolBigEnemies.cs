using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolBigEnemies : MonoBehaviour
{
    //Singleton
    public static ObjectPoolBigEnemies SharedInstance;

    //List of pooled objects
    public List<GameObject> PooledObjects;

    //Objects to pool
    public int AmountToPool;
    public GameObject StageOneBigEnemyPrefab;
    public GameObject StageTwoBigEnemyPrefab;

    //Event Channel
    public IntEventChannelSO StageChangeIntEventChannel;


    private void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }

        StageChangeIntEventChannel.OnEventRaised += SetupPool;
    }

    private void OnDestroy()
    {
        StageChangeIntEventChannel.OnEventRaised -= SetupPool;
    }

    private void Start()
    {
        SetupPool(stage: 1);
    }

    private void SetupPool(int stage)
    {
        DestroyPooledGameObjects();

        PooledObjects = new List<GameObject>();

        GameObject objectToPool;
        switch (stage)
        {
            case 0:
                objectToPool = null;
                break;
            case 1:
                objectToPool = StageOneBigEnemyPrefab;
                break;
            case 2:
                objectToPool = StageTwoBigEnemyPrefab;
                break;
            default:
                objectToPool = StageOneBigEnemyPrefab;
                break;
        }
        if (objectToPool == null) return;

        GameObject tempPoolItem;
        for (int i = 0; i < AmountToPool; i++)
        {
            tempPoolItem = Instantiate(objectToPool);
            tempPoolItem.SetActive(false);
            tempPoolItem.transform.parent = this.transform;
            PooledObjects.Add(tempPoolItem);
        }
    }

    private void DestroyPooledGameObjects()
    {
        if (PooledObjects.Count > 0)
        {
            foreach (GameObject pooledObject in PooledObjects)
            {
                Destroy(pooledObject);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < PooledObjects.Count; i++)
        {
            if (!PooledObjects[i].activeInHierarchy)
            {
                return PooledObjects[i];
            }
        }
        return null;
    }
}
