using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IHealable
{
    [Header("Movement Handling")]
    [SerializeField] PlayerInputHandler _playerInputHandler;
    [SerializeField] Rigidbody2D _rigidBody;

    [Header("Animation")]
    [SerializeField] Animator _thrusterAnimator;
    [SerializeField] MessagePopupController _messagePopupController;

    [Header("Damage Flash")]
    [SerializeField] SpriteRenderer _shipSpriteRenderer;
    [SerializeField] Material _damageFlashMaterial;
    [SerializeField] Material _defaultShipMaterial;
    private float _damageFlashDuration = 0.1f;

    [Header("Ship Stats")]
    [SerializeField] ShipStats _shipStats;

    [Header("Shield")]
    [SerializeField] SpriteRenderer _shieldSpriteRenderer;

    [Header("Audio")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _playerShotSound;

    [Header("Shot Prefabs")]
    public GameObject SingleShotPrefab;
    public GameObject DoubleShotPrefab;
    public GameObject TripleShotPrefab;
    public GameObject QuadShotPrefab;
    public GameObject SuperTripleShotPrefab;
    public GameObject FireShotPrefab;
    public GameObject PlasmaShotPrefab;
    public GameObject LaserShotPrefab;

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
    private float _nextTimeToReload = 0f;
    private float _nextTimeToFire = 0f;

    //Buffs
    private bool _hasShield = false;

    //Debuffs
    private bool _mirrorControls = false;

    private void Start()
    {
        SetGunType(_shipStats.gunType, true, false);
        SetStats();

        _shipSpriteRenderer.material = _defaultShipMaterial;

        _audioSource.clip = _playerShotSound;
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
        MaxHitPoints = _shipStats.hitPoints;
        MoveSpeed = _shipStats.moveSpeed;
        MaxBullets = _shipStats.maxBullets;
        FireRate = _shipStats.fireRate;
        ProjectileSpeed = _shipStats.projectileSpeed;
        ReloadRate = _shipStats.reloadRate;        

        MovespeedUpgradeLevel = PlayerPrefs.GetInt(Constants.MOVESPEED_UPGRADE_LEVEL);
        BulletUpgradeLevel = PlayerPrefs.GetInt(Constants.BULLET_UPGRADE_LEVEL);
        GunUpgradeLevel = PlayerPrefs.GetInt(Constants.GUN_UPGRADE_LEVEL);
        ApplyUpgrades(MovespeedUpgradeLevel, BulletUpgradeLevel, GunUpgradeLevel);

        CurrentHitPoints = MaxHitPoints;
        OnHealthValueChange?.Invoke(this, EventArgs.Empty);

        CurrentBullets = MaxBullets;
        OnBulletValueChange?.Invoke(this, EventArgs.Empty);

        Money = PlayerPrefs.GetInt(Constants.MONEY_AMOUNT);
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
            _messagePopupController.PlayMessage("Stage 1");
            return;
        }

        if (!sendMessage) return;

        switch (gunType)
        {
            case GunTypeEnum.singleShot:
                _messagePopupController.PlayMessage("Single Shot");
                break;
            case GunTypeEnum.doubleShot:
                _messagePopupController.PlayMessage("Double Shot");
                break;
            case GunTypeEnum.tripleShot:
                _messagePopupController.PlayMessage("Triple Shot");
                break;
            case GunTypeEnum.quadShot:
                _messagePopupController.PlayMessage("Quad Shot");
                break;
            case GunTypeEnum.superTripleShot:
                _messagePopupController.PlayMessage("Super Triple Shot");
                break;
            case GunTypeEnum.fireShot:
                _messagePopupController.PlayMessage("Fire Shot");
                break;
            case GunTypeEnum.plasmaShot:
                _messagePopupController.PlayMessage("Plasma Shot");
                break;
            case GunTypeEnum.laserShot:
                _messagePopupController.PlayMessage("Laser Shot");
                break;
            default:
                _messagePopupController.PlayMessage("Error Happened");
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
        float horizontalInput = _playerInputHandler.movementInput.x;
        float verticalInput = _playerInputHandler.movementInput.y;

        if (_mirrorControls)
        {
            horizontalInput *= -1f;
            verticalInput *= -1f;
        }

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize();

        _rigidBody.velocity = movement * MoveSpeed;
    }

    private void HandleReload()
    {
        //If the magazine is not full and its time to reload, perform reload and reset the reload timer
        if (CurrentBullets < MaxBullets && _nextTimeToReload <= Time.timeSinceLevelLoad)
        {
            _nextTimeToReload = Time.timeSinceLevelLoad + (1f / ReloadRate);
            CurrentBullets++;
            OnBulletValueChange?.Invoke(this, EventArgs.Empty);
        }

        //If the magazine is full also reset the reload timer, so the player cant instatly reload after shooting one bullet
        if (CurrentBullets == MaxBullets)
        {
            _nextTimeToReload = Time.timeSinceLevelLoad + (1f / ReloadRate);
        }
    }

    private void HandleShoot()
    {
        if (_playerInputHandler.shootInput && _nextTimeToFire <= Time.timeSinceLevelLoad && CurrentBullets > 0)
        {
            _nextTimeToFire = Time.timeSinceLevelLoad + (1f / FireRate);
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

        _audioSource.pitch = UnityEngine.Random.Range(0.85f, 1.15f);
        _audioSource.Play();

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

        if (_hasShield)
        {
            RemoveShield();
            return;
        }

        CurrentHitPoints -= damageAmount;
        OnHealthValueChange?.Invoke(this, EventArgs.Empty);
        if (CurrentHitPoints <= 0)
        {
            StartCoroutine(GameOver());
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
        _shipSpriteRenderer.material = _damageFlashMaterial;
        yield return new WaitForSeconds(_damageFlashDuration);
        _shipSpriteRenderer.material = _defaultShipMaterial;
    }

    //Functions to increase stats from Drops or ShopUpgrades
    public void IncreaseSpeed(bool sendMessage)
    {
        MoveSpeed += 0.5f;
        OnMovespeedValueChange?.Invoke(this, EventArgs.Empty);
        
        if (!sendMessage) return;
        _messagePopupController.PlayMessage(Constants.SPEED_UPGRADE_TEXT);
    }

    public void IncreaseBullet(bool sendMessage)
    {
        MaxBullets += 1;
        OnBulletValueChange?.Invoke(this, EventArgs.Empty);

        if (!sendMessage) return;
        _messagePopupController.PlayMessage(Constants.BULLET_UPGRADE_TEXT);
    }

    private void UpgradeGun()
    {
        //TODO: Make not overflow
        CurrentGunType++;
    }

    public void AddMoney(int money)
    {
        Money += money;
        PlayerPrefs.SetInt(Constants.MONEY_AMOUNT, Money);
        OnMoneyValueChange?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveMoney(int money)
    {
        Money -= money;
        PlayerPrefs.SetInt(Constants.MONEY_AMOUNT, Money);
        OnMoneyValueChange?.Invoke(this, EventArgs.Empty);
    }

    public void AddShield()
    {
        _hasShield = true;
        _shieldSpriteRenderer.enabled = true;
        _messagePopupController.PlayMessage("Shield Activated");
    }

    public void RemoveShield()
    {
        _hasShield = false;
        _shieldSpriteRenderer.enabled = false;
    }

    //Functions to debuff stats from MalusDrops
    public void DebuffMovementSpeed()
    {
        MoveSpeed = _shipStats.moveSpeed * 0.75f;
        _thrusterAnimator.Play(Constants.THRUSTER_ANIMATION_SLOW);
        OnMovespeedValueChange?.Invoke(this, EventArgs.Empty);
        StartCoroutine(ResetMovementSpeed());
        _messagePopupController.PlayMessage("Engine Failure");
    }

    private IEnumerator ResetMovementSpeed()
    {
        yield return new WaitForSeconds(5f);
        MoveSpeed = _shipStats.moveSpeed;
        _messagePopupController.PlayMessage("Engine Repaired");
        _thrusterAnimator.Play(Constants.THRUSTER_ANIMATION);
        OnMovespeedValueChange?.Invoke(this, EventArgs.Empty);
    }

    public void DebuffMirrorControls()
    {
        _mirrorControls = true;
        _messagePopupController.PlayMessage("Mirror Controls");
        StartCoroutine(ResetMirrorControls());
    }

    private IEnumerator ResetMirrorControls()
    {
        yield return new WaitForSeconds(5f);
        _messagePopupController.PlayMessage("Controls Normalized");
        _mirrorControls = false;
    }

    //Game Over
    private IEnumerator GameOver()
    {
        _messagePopupController.PlayMessage("Game Over");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
    }
}