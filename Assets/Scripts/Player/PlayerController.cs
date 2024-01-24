using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHealable
{
    //Input Handling and Movement
    [SerializeField] PlayerInputHandler playerInputHandler;
    [SerializeField] Rigidbody2D rb;

    //Shipstats Scriptable Object
    [SerializeField] ShipStats shipStats;

    //Prefabs
    public GameObject singleShotPrefab;
    public GameObject doubleShotPrefab;
    public GameObject tripleShotPrefab;
    public GameObject quadShotPrefab;
    public GameObject superTripleShotPrefab;
    public GameObject fireShotPrefab;
    public GameObject plasmaShotPrefab;
    public GameObject laserShotPrefab;

    //Properties
    public float MaxHitPoints { get; private set; }
    public float MoveSpeed { get; private set; }
    public int MaxBullets { get; private set; }
    public float FireRate { get; private set; }
    public float ProjectileSpeed { get; private set; }
    public float ReloadRate { get; private set; }
    public GunTypeEnum CurrentGunType { get; private set; }

    //Events
    public event EventHandler OnGunTypeChange;

    //Timers
    private float nextTimeToReload = 0f;
    private float nextTimeToFire = 0f;

    //Current Player Stats
    private int currentBullets;
    public float currentHitPoints;

    private void Start()
    {
        SetStats();
        SetGunType(shipStats.gunType);
    }

    private void Update()
    {
        HandleMovement();
        HandleShoot();
        HandleReload();
    }

    //Stats
    private void SetStats()
    {
        MaxHitPoints = shipStats.hitPoints;
        MoveSpeed = shipStats.moveSpeed;
        MaxBullets = shipStats.maxBullets;
        FireRate = shipStats.fireRate;
        ProjectileSpeed = shipStats.projectileSpeed;
        ReloadRate = shipStats.reloadRate;

        currentBullets = MaxBullets;
        currentHitPoints = MaxHitPoints;
    }

    public void SetGunType(GunTypeEnum gunType)
    {
        CurrentGunType = gunType;
        OnGunTypeChange?.Invoke(this, EventArgs.Empty);
    }

    //Handlers
    private void HandleMovement()
    {
        float horizontalInput = playerInputHandler.movementInput.x;
        float verticalInput = playerInputHandler.movementInput.y;

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize(); 

        rb.velocity = movement * MoveSpeed;
    }

    private void HandleReload()
    {
        //If the magazine is not full and its time to reload, perform reload and reset the reload timer
        if (currentBullets < MaxBullets && nextTimeToReload <= Time.time)
        {
            nextTimeToReload = Time.time + (1f / ReloadRate);
            currentBullets++;
        }

        //If the magazine is full also reset the reload timer, so the player cant instatly reload after shooting one bullet
        if (currentBullets == MaxBullets)
        {
            nextTimeToReload = Time.time + (1f / ReloadRate);
        }
    }

    private void HandleShoot()
    {
        if (playerInputHandler.shootInput && nextTimeToFire <= Time.time && currentBullets > 0)
        {
            nextTimeToFire = Time.time + (1f / FireRate);
            currentBullets--;

            switch (CurrentGunType)
            {
                case GunTypeEnum.singleShot:
                    InstantiatePlayerProjectile();
                    break;
                case GunTypeEnum.doubleShot:
                    InstantiatePlayerProjectile();
                    break;
                case GunTypeEnum.tripleShot:
                    InstantiatePlayerProjectile();
                    break;
                case GunTypeEnum.quadShot:
                    InstantiatePlayerProjectile();
                    break;
                case GunTypeEnum.superTripleShot:
                    InstantiatePlayerProjectile();
                    break;
                case GunTypeEnum.fireShot:
                    InstantiatePlayerProjectile();
                    break;
                case GunTypeEnum.plasmaShot:
                    InstantiatePlayerProjectile();
                    break;
                case GunTypeEnum.laserShot:
                    InstantiatePlayerProjectile();
                    break;
                default:
                    break;
            }
        }
    }

    private void InstantiatePlayerProjectile()
    {
        GameObject playerProjectile = ObjectPoolPlayerProjectiles.SharedInstance.GetPooledObject();
        playerProjectile.transform.position = this.transform.position;
        playerProjectile.transform.rotation = this.transform.rotation;
        playerProjectile.SetActive(true);
    }

    //Game Over
    private void GameOver()
    {
        Debug.Log("Game Over");
    }

    //Interface Implementation
    public void TakeDamage(float damageAmount)
    {
        currentHitPoints -= damageAmount;
        if (currentHitPoints <= 0)
        {
            Destroy(this.gameObject);
            GameOver();
        }
    }

    public void ProvideHealing(float healAmount)
    {
        currentHitPoints += healAmount;
        if (currentHitPoints > MaxHitPoints)
        {
            currentHitPoints = MaxHitPoints;
        }
    }  

    //Functions to increase stats from Drops or ShopUpgrades
    public void IncreaseSpeed()
    {
        MoveSpeed += 0.5f;
    }

    public void IncreaseBullet()
    {
        MaxBullets += 1;
    }

    public void IncreaseHitpoints()
    {
        MaxHitPoints += 1;
        currentHitPoints += 1;
    }
}
