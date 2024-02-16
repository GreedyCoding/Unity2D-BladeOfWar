using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolPlayerProjectiles : MonoBehaviour
{
    //Singleton
    public static ObjectPoolPlayerProjectiles SharedInstance;

    //Reference
    [SerializeField] PlayerController _playerController;

    //Event Channel
    [SerializeField] GunTypeEventChannelSO _gunChangeVoidEventChannelSO;
    
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
        InitialPoolSetup();
        _gunChangeVoidEventChannelSO.OnEventRaised += SetObjectAndAmount;
        _gunChangeVoidEventChannelSO.OnEventRaised += SetupPool;
    }

    void OnDisable()
    {
        _gunChangeVoidEventChannelSO.OnEventRaised -= SetObjectAndAmount;
        _gunChangeVoidEventChannelSO.OnEventRaised -= SetupPool;
    }

    private void InitialPoolSetup()
    {
        SetObjectAndAmount(_playerController.CurrentGunType);
        SetupPool(_playerController.CurrentGunType);
    }

    void SetupPool(GunTypeEnum enumerator)
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
            tempPoolItem.transform.parent = this.transform;
            pooledObjects.Add(tempPoolItem);
        }
    }

    void SetObjectAndAmount(GunTypeEnum enumerator)
    {
        switch (enumerator)
        {
            case GunTypeEnum.singleShot:
                objectToPool = _playerController.SingleShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.doubleShot:
                objectToPool = _playerController.DoubleShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.tripleShot:
                objectToPool = _playerController.TripleShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.quadShot:
                objectToPool = _playerController.QuadShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.superTripleShot:
                objectToPool = _playerController.SuperTripleShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.fireShot:
                objectToPool = _playerController.FireShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.plasmaShot:
                objectToPool = _playerController.PlasmaShotPrefab;
                amountToPool = 10;
                break;
            case GunTypeEnum.laserShot:
                objectToPool = _playerController.LaserShotPrefab;
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
