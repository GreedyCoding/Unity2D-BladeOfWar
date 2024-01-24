using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolPlayerProjectiles : MonoBehaviour
{
    //Singleton
    public static ObjectPoolPlayerProjectiles SharedInstance;

    //List of pooled objects
    public List<GameObject> pooledObjects;

    //Objects to pool
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        if(SharedInstance == null)
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
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
