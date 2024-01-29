using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IHealable
{
    [Header("Movement Handling")]
    [SerializeField] PlayerInputHandler playerInputHandler;
    [SerializeField] Rigidbody2D rb;

    [Header("Animation")]
    [SerializeField] Animator thrusterAnimator;
    [SerializeField] MessagePopupController messagePopupController;

    [Header("Damage Flash")]
    [SerializeField] SpriteRenderer shipSpriteRenderer;
    [SerializeField] Material damageFlashMaterial;
    [SerializeField] Material defaultShipMaterial;
    private float damageFlashDuration = 0.1f;

    [Header("Ship Stats")]
    [SerializeField] ShipStats shipStats;

    [Header("Shield")]
    [SerializeField] SpriteRenderer shieldSpriteRenderer;

    [Header("Shot Prefabs")]
    public GameObject singleShotPrefab;
    public GameObject doubleShotPrefab;
    public GameObject tripleShotPrefab;
    public GameObject quadShotPrefab;
    public GameObject superTripleShotPrefab;
    public GameObject fireShotPrefab;
    public GameObject plasmaShotPrefab;
    public GameObject laserShotPrefab;

    //Health
    public int MaxHitPoints { get; private set; }
    public int CurrentHitPoints { get; private set; }
    //Movespeed
    public float MoveSpeed { get; private set; }
    //Weapon
    public int MaxBullets { get; private set; }
    public int CurrentBullets { get; private set; }
    public float FireRate { get; private set; }
    public float ProjectileSpeed { get; private set; }
    public float ReloadRate { get; private set; }
    public GunTypeEnum CurrentGunType { get; private set; }
    //Upgrades and Money
    public int Money { get; private set; }
    public int MovespeedUpgradeLevel { get; private set; }
    public int BulletUpgradeLevel { get; private set; }
    public int GunUpgradeLevel { get; private set; }


    //Events
    public event EventHandler OnGunTypeChange;
    public event EventHandler OnHealthValueChange;
    public event EventHandler OnBulletValueChange;
    public event EventHandler OnMovespeedValueChange;
    public event EventHandler OnMoneyValueChange;
 
    //Timers
    private float nextTimeToReload = 0f;
    private float nextTimeToFire = 0f;

    //Buffs
    private bool hasShield = false;

    //Debuffs
    private bool mirrorControls = false;

    private void Start()
    {
        SetGunType(shipStats.gunType, true, false);
        SetStats();
        shipSpriteRenderer.material = defaultShipMaterial;
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

        /*
        PlayerPrefs.SetInt("MovespeedUpgradeLevel", 0);
        PlayerPrefs.SetInt("BulletUpgradeLevel", 0);
        PlayerPrefs.SetInt("GunUpgradeLevel", 0);
        PlayerPrefs.SetInt("Money", 0);
        */

        MovespeedUpgradeLevel = PlayerPrefs.GetInt("MovespeedUpgradeLevel");
        BulletUpgradeLevel = PlayerPrefs.GetInt("BulletUpgradeLevel");
        GunUpgradeLevel = PlayerPrefs.GetInt("GunUpgradeLevel");
        ApplyUpgrades(MovespeedUpgradeLevel, BulletUpgradeLevel, GunUpgradeLevel);

        CurrentHitPoints = MaxHitPoints;
        OnHealthValueChange?.Invoke(this, EventArgs.Empty);

        CurrentBullets = MaxBullets;
        OnBulletValueChange?.Invoke(this, EventArgs.Empty);

        Money = PlayerPrefs.GetInt("Money");
        OnMoneyValueChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetGunType(GunTypeEnum gunType, bool initialSet, bool sendMessage)
    {
        if(CurrentGunType == gunType && !initialSet)
        {
            IncreaseBullet(true);
            return;
        }

        CurrentGunType = gunType;
        OnGunTypeChange?.Invoke(this, EventArgs.Empty);

        if (initialSet)
        {
            messagePopupController.PlayMessage("Stage 1");
            return;
        }

        if (!sendMessage) return;

        switch (gunType)
        {
            case GunTypeEnum.singleShot:
                messagePopupController.PlayMessage("Single Shot");
                break;
            case GunTypeEnum.doubleShot:
                messagePopupController.PlayMessage("Double Shot");
                break;
            case GunTypeEnum.tripleShot:
                messagePopupController.PlayMessage("Triple Shot");
                break;
            case GunTypeEnum.quadShot:
                messagePopupController.PlayMessage("Quad Shot");
                break;
            case GunTypeEnum.superTripleShot:
                messagePopupController.PlayMessage("Super Triple Shot");
                break;
            case GunTypeEnum.fireShot:
                messagePopupController.PlayMessage("Fire Shot");
                break;
            case GunTypeEnum.plasmaShot:
                messagePopupController.PlayMessage("Plasma Shot");
                break;
            case GunTypeEnum.laserShot:
                messagePopupController.PlayMessage("Laser Shot");
                break;
            default:
                messagePopupController.PlayMessage("Error Happened");
                break;
        }
    }

    private void ApplyUpgrades(int movespeedUpgradeLevel, int bulletUpgradeLevel, int gunUpgradeLevel)
    {
        for (int i = 0; i < movespeedUpgradeLevel; i++)
        {
            IncreaseSpeed(false);
        }

        for (int i = 0; i < bulletUpgradeLevel; i++)
        {
            IncreaseBullet(false);
        }

        for (int i = 0; i < gunUpgradeLevel; i++)
        {
            UpgradeGun();
        }
    }

    //Handlers
    private void HandleMovement()
    {
        float horizontalInput = playerInputHandler.movementInput.x;
        float verticalInput = playerInputHandler.movementInput.y;

        if (mirrorControls)
        {
            horizontalInput *= -1f;
            verticalInput *= -1f;
        }

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize();

        rb.velocity = movement * MoveSpeed;
    }

    private void HandleReload()
    {
        //If the magazine is not full and its time to reload, perform reload and reset the reload timer
        if (CurrentBullets < MaxBullets && nextTimeToReload <= Time.timeSinceLevelLoad)
        {
            nextTimeToReload = Time.timeSinceLevelLoad + (1f / ReloadRate);
            CurrentBullets++;
            OnBulletValueChange?.Invoke(this, EventArgs.Empty);
        }

        //If the magazine is full also reset the reload timer, so the player cant instatly reload after shooting one bullet
        if (CurrentBullets == MaxBullets)
        {
            nextTimeToReload = Time.timeSinceLevelLoad + (1f / ReloadRate);
        }
    }

    private void HandleShoot()
    {
        if (playerInputHandler.shootInput && nextTimeToFire <= Time.timeSinceLevelLoad && CurrentBullets > 0)
        {
            nextTimeToFire = Time.timeSinceLevelLoad + (1f / FireRate);
            CurrentBullets--;
            OnBulletValueChange?.Invoke(this, EventArgs.Empty);

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

        //Handle children of the different projectiles
        if(CurrentGunType == GunTypeEnum.doubleShot || CurrentGunType == GunTypeEnum.quadShot)
        {
            foreach (Transform child in playerProjectile.transform)
            {
                child.gameObject.SetActive(true);
                child.gameObject.transform.position = new Vector2(child.gameObject.transform.position.x, this.transform.position.y);
            }
        }
        else if (CurrentGunType == GunTypeEnum.tripleShot)
        {
            foreach (Transform child in playerProjectile.transform)
            {
                child.gameObject.SetActive(true);
                child.gameObject.transform.position = this.transform.position;
            }
        }

    }

    //Interface Implementation
    public void TakeDamage(int damageAmount)
    {
        StartCoroutine(DamageFlash());

        if (hasShield)
        {
            RemoveShield();
            return;
        }

        CurrentHitPoints -= damageAmount;
        OnHealthValueChange?.Invoke(this, EventArgs.Empty);
        if (CurrentHitPoints <= 0)
        {
            Destroy(this.gameObject);
            GameOver();
        }
    }

    public void ProvideHealing(int healAmount)
    {
        CurrentHitPoints += healAmount;

        if (CurrentHitPoints > MaxHitPoints)
        {
            CurrentHitPoints = MaxHitPoints;
            AddShield();
        }

        OnHealthValueChange?.Invoke(this, EventArgs.Empty);
    }

    public void TakeDamage(float damageAmount)
    {
        TakeDamage((int)damageAmount);
    }

    public void ProvideHealing(float healAmount)
    {
        ProvideHealing((int)healAmount);
    }

    //Flash the ship white when taking damage
    private IEnumerator DamageFlash()
    {
        shipSpriteRenderer.material = damageFlashMaterial;
        yield return new WaitForSeconds(damageFlashDuration);
        shipSpriteRenderer.material = defaultShipMaterial;
    }

    //Functions to increase stats from Drops or ShopUpgrades
    public void IncreaseSpeed(bool sendMessage)
    {
        MoveSpeed += 0.5f;
        OnMovespeedValueChange?.Invoke(this, EventArgs.Empty);
        
        if (!sendMessage) return;
        messagePopupController.PlayMessage("Extra Speed");
    }

    public void IncreaseBullet(bool sendMessage)
    {
        MaxBullets += 1;
        OnBulletValueChange?.Invoke(this, EventArgs.Empty);

        if (!sendMessage) return;
        messagePopupController.PlayMessage("Extra Bullet");
    }

    private void UpgradeGun()
    {
        //TODO: Make not overflow
        CurrentGunType++;
    }

    public void AddMoney(int money)
    {
        Money += money;
        PlayerPrefs.SetInt("Money", Money);
        OnMoneyValueChange?.Invoke(this, EventArgs.Empty);
    }

    public void AddShield()
    {
        hasShield = true;
        shieldSpriteRenderer.enabled = true;
        messagePopupController.PlayMessage("Shield Activated");
    }

    public void RemoveShield()
    {
        hasShield = false;
        shieldSpriteRenderer.enabled = false;
    }

    //Functions to debuff stats from MalusDrops
    public void DebuffMovementSpeed()
    {
        MoveSpeed = shipStats.moveSpeed * 0.75f;
        thrusterAnimator.Play("Thruster Animation Slow");
        OnMovespeedValueChange?.Invoke(this, EventArgs.Empty);
        StartCoroutine(ResetMovementSpeed());
        messagePopupController.PlayMessage("Engine Failure");
    }

    private IEnumerator ResetMovementSpeed()
    {
        yield return new WaitForSeconds(5f);
        MoveSpeed = shipStats.moveSpeed;
        messagePopupController.PlayMessage("Engine Repaired");
        thrusterAnimator.Play("Thruster Animation");
        OnMovespeedValueChange?.Invoke(this, EventArgs.Empty);
    }

    public void DebuffMirrorControls()
    {
        mirrorControls = true;
        messagePopupController.PlayMessage("Mirror Controls");
        StartCoroutine(ResetMirrorControls());
    }

    private IEnumerator ResetMirrorControls()
    {
        yield return new WaitForSeconds(5f);
        messagePopupController.PlayMessage("Controls Normalized");
        mirrorControls = false;
    }

    //Game Over
    private void GameOver()
    {
        SceneManager.LoadScene("Game Scene");
    }
}
