using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolPlayerProjectiles : MonoBehaviour
{
    //Singleton
    public static ObjectPoolPlayerProjectiles SharedInstance;

    //Reference
    [SerializeField] PlayerController playerController;
    
    //List of pooled objects
    public List<GameObject> pooledObjects;

    //Objects to pool
    private GameObject objectToPool;
    private int amountToPool;

    void Awake()
    {
        if(SharedInstance == null)
        {
            SharedInstance = this;
        }

    }

    void Start()
    {
        SetObjectAndAmount();
        SetupPool();
        playerController.OnGunTypeChange += SetObjectAndAmount;
        playerController.OnGunTypeChange += SetupPool;
    }

    void OnDisable()
    {
        playerController.OnGunTypeChange -= SetObjectAndAmount;
        playerController.OnGunTypeChange -= SetupPool;
    }

    void SetupPool(object sender, EventArgs e)
    {
        SetupPool();
    }

    void SetupPool()
    {
        if(pooledObjects.Count > 0)
        {
            foreach(GameObject pooledObject in pooledObjects)
            {
                Destroy(pooledObject);
            }
        }

        pooledObjects = new List<GameObject>();
        GameObject tempPoolItem;
        for (int i = 0; i < amountToPool; i++)
        {
            tempPoolItem = Instantiate(objectToPool);
            tempPoolItem.SetActive(false);
            pooledObjects.Add(tempPoolItem);
        }
    }

    void SetObjectAndAmount(object sender, EventArgs e)
    {
        SetObjectAndAmount();
    }

    void SetObjectAndAmount()
    {
        switch (playerController.CurrentGunType)
        {
            case GunTypeEnum.singleShot:
                objectToPool = playerController.SingleShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.doubleShot:
                objectToPool = playerController.DoubleShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.tripleShot:
                objectToPool = playerController.TripleShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.quadShot:
                objectToPool = playerController.QuadShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.superTripleShot:
                objectToPool = playerController.SuperTripleShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.fireShot:
                objectToPool = playerController.FireShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.plasmaShot:
                objectToPool = playerController.PlasmaShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.laserShot:
                objectToPool = playerController.LaserShotPrefab;
                amountToPool = 10;
                break;
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
