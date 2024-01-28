using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolBeetleEnemies : MonoBehaviour
{
    //Singleton
    public static ObjectPoolBeetleEnemies SharedInstance;

    //List of pooled objects
    public List<GameObject> pooledObjects;

    //Objects to pool
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
    }

    void Start()
    {
        SetupPool();
    }

    void SetupPool()
    {
        pooledObjects = new List<GameObject>();
        GameObject tempPoolItem;
        for (int i = 0; i < amountToPool; i++)
        {
            tempPoolItem = Instantiate(objectToPool);
            tempPoolItem.SetActive(false);
            pooledObjects.Add(tempPoolItem);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

}
