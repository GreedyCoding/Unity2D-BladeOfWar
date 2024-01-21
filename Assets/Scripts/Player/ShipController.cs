using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour, IHealable
{
    //Input Handling and Movement
    [SerializeField] PlayerInputHandler playerInputHandler;
    [SerializeField] Rigidbody2D rb;

    //Shipstats Scriptable Object
    [SerializeField] ShipStats shipStats;

    //Prefabs
    [SerializeField] GameObject singleShotPrefab;
    [SerializeField] GameObject doubleShotPrefab;
    [SerializeField] GameObject tripleShotPrefab;
    [SerializeField] GameObject quadShotPrefab;
    [SerializeField] GameObject superTripleShotPrefab;
    [SerializeField] GameObject fireShotPrefab;
    [SerializeField] GameObject plasmaShotPrefab;
    [SerializeField] GameObject laserShotPrefab;

    //Properties
    public float MaxHitPoints { get; private set; }
    public float MoveSpeed { get; private set; }
    public int MaxBullets { get; private set; }
    public float FireRate { get; private set; }
    public float ProjectileSpeed { get; private set; }
    public float ReloadRate { get; private set; }
    public GunTypeEnum CurrentGunType { get; private set; }

    //Timers
    private float nextTimeToReload = 0f;
    private float nextTimeToFire = 0f;

    //Current Player Stats
    private int currentBullets;
    private float currentHitPoints;

    private void Start()
    {
        SetShipStats();
    }

    private void Update()
    {
        HandleMovement();
        HandleShoot();
        HandleReload();
    }

    private void SetShipStats()
    {
        MaxHitPoints = shipStats.hitPoints;
        MoveSpeed = shipStats.moveSpeed;
        MaxBullets = shipStats.maxBullets;
        FireRate = shipStats.fireRate;
        ProjectileSpeed = shipStats.projectileSpeed;
        ReloadRate = shipStats.reloadRate;
        CurrentGunType = shipStats.gunType;

        currentBullets = MaxBullets;
        currentHitPoints = MaxHitPoints;
    }

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
                    Instantiate(singleShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.doubleShot:
                    Instantiate(doubleShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.tripleShot:
                    Instantiate(tripleShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.quadShot:
                    Instantiate(quadShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.superTripleShot:
                    Instantiate(superTripleShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.fireShot:
                    Instantiate(fireShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.plasmaShot:
                    Instantiate(plasmaShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.laserShot:
                    Instantiate(laserShotPrefab, this.transform.position, this.transform.rotation);
                    break;
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
    }

    public void TakeDamage(float damageAmount)
    {
        currentHitPoints -= damageAmount;
        if (MaxHitPoints <= 0)
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

    
}
