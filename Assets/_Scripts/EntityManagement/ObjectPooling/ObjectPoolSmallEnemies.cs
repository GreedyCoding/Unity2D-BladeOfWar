using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolSmallEnemies : MonoBehaviour
{
    //Singleton
    public static ObjectPoolSmallEnemies SharedInstance;

    //List of pooled objects
    public List<GameObject> PooledObjects;

    //Objects to pool
    public int AmountToPool;
    public GameObject StageOneSmallEnemyPrefab;
    public GameObject StageTwoSmallEnemyPrefab;

    //Event Channel
    public IntEventChannelSO StageChangeIntEventChannel;

    private void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
    }

    private void Start()
    {
        StageChangeIntEventChannel.OnEventRaised += SetupPool;

        SetupPool(stage: 1);
    }

    private void OnDestroy()
    {
        StageChangeIntEventChannel.OnEventRaised -= SetupPool;
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
                objectToPool = StageOneSmallEnemyPrefab;
                break;
            case 2:
                objectToPool = StageTwoSmallEnemyPrefab;
                break;
            default:
                objectToPool = StageOneSmallEnemyPrefab;
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
