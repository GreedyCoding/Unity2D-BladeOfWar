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
    [SerializeField] GameObject _deathExplosionOne;
    [SerializeField] GameObject _deathExplosionTwo;
    [SerializeField] GameObject _deathExplosionThree;

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

    [Header("UI")]
    [SerializeField] GameUserInterfaceController _gameUserInterfaceController;  

    [Header("Void Event Channel SO")]
    [SerializeField] IntToupleEventChannelSO _bulletChangeVoidEventChannelSO;
    [SerializeField] GunTypeEventChannelSO _gunChangeEventChannelSO;
    [SerializeField] IntEventChannelSO _healthChangeEventChannelSO;
    [SerializeField] IntEventChannelSO _moneyChangeEventChannelSO;
    [SerializeField] FloatEventChannelSO _movespeedChangeEventChannelSO;

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
    public float MaxMoveSpeed { get; private set; }
    public float CurrentMoveSpeed { get; private set; }

    //Bullets
    public int MaxBullets { get; private set; }
    public int CurrentBullets { get; private set; }

    //Weapon
    public float FireRate { get; private set; }
    public float ProjectileSpeed { get; private set; }
    public float ReloadRate { get; private set; }
    public GunTypeEnum CurrentGunType { get; private set; }

    //Upgrades and Money
    public int Money { get; private set; }
    public int MovespeedUpgradeLevel { get; private set; }
    public int BulletUpgradeLevel { get; private set; }
    public int GunUpgradeLevel { get; private set; }
    
    //Timers
    private float _nextTimeToReload = 0f;
    private float _nextTimeToFire = 0f;

    //Buffs
    private bool _hasShield = false;

    //Debuffs
    private bool _mirrorControls = false;

    private void Start()
    {
        SetGunType(_shipStats.GunType, true, false);
        SetStats();

        _shipSpriteRenderer.material = _defaultShipMaterial;

        _audioSource.clip = _playerShotSound;
    }

    private void Update()
    {
        HandleMovement();
        HandleShoot();
        HandleReload();
        HandlePauseGameInput();
    }

    #region Events

    private void RaisePlayerStatsChangedEvents()
    {
        //Fire to initialize the object pool
        _gunChangeEventChannelSO.RaiseEvent(CurrentGunType);

        //Fire to update UI
        _healthChangeEventChannelSO.RaiseEvent(CurrentHitPoints);
        _bulletChangeVoidEventChannelSO.RaiseEvent(CurrentBullets, MaxBullets);
        _moneyChangeEventChannelSO.RaiseEvent(PlayerPrefs.GetInt(Constants.MONEY_AMOUNT));
    }

    #endregion

    #region Stats

    private void SetStats()
    {
        //Set Standard Stats
        MaxHitPoints = _shipStats.HitPoints;
        MaxMoveSpeed = _shipStats.MoveSpeed;
        MaxBullets = _shipStats.MaxBullets;
        FireRate = _shipStats.FireRate;
        ProjectileSpeed = _shipStats.ProjectileSpeed;
        ReloadRate = _shipStats.ReloadRate;

        //Get and Apply Upgrades
        MovespeedUpgradeLevel = PlayerPrefs.GetInt(Constants.MOVESPEED_UPGRADE_LEVEL);
        BulletUpgradeLevel = PlayerPrefs.GetInt(Constants.BULLET_UPGRADE_LEVEL);
        GunUpgradeLevel = PlayerPrefs.GetInt(Constants.GUN_UPGRADE_LEVEL);
        ApplyUpgrades(MovespeedUpgradeLevel, BulletUpgradeLevel, GunUpgradeLevel);

        //Set current stats
        CurrentHitPoints = MaxHitPoints;
        CurrentBullets = MaxBullets;
        CurrentMoveSpeed = MaxMoveSpeed;
        Money = PlayerPrefs.GetInt(Constants.MONEY_AMOUNT);

        RaisePlayerStatsChangedEvents();
    }

    public void SetShopUpgrades()
    {
        int lastMovespeedUpgradeLevel = MovespeedUpgradeLevel;
        int lastBulletUpgradeLevel = BulletUpgradeLevel;
        int lastGunUpgradeLevel = GunUpgradeLevel;

        int currentMoveSpeedUpgradeLevel = PlayerPrefs.GetInt(Constants.MOVESPEED_UPGRADE_LEVEL);
        int currentBulletUpgradLevel = PlayerPrefs.GetInt(Constants.BULLET_UPGRADE_LEVEL);
        int currentGunUpgradeLevel = PlayerPrefs.GetInt(Constants.GUN_UPGRADE_LEVEL);

        ApplyUpgrades(currentMoveSpeedUpgradeLevel - lastMovespeedUpgradeLevel, 
                      currentBulletUpgradLevel - lastBulletUpgradeLevel,
                      currentGunUpgradeLevel - lastGunUpgradeLevel);

        RaisePlayerStatsChangedEvents();
    }

    public void SetGunType(GunTypeEnum gunType, bool initialSet, bool sendMessage)
    {
        if(CurrentGunType == gunType && !initialSet)
        {
            IncreaseBullet(true);
            return;
        }

        CurrentGunType = gunType;
        _gunChangeEventChannelSO.RaiseEvent(CurrentGunType);

        if (initialSet)
        {
            MessagePopupController.Instance.PlayMessage("Stage 1");
            return;
        }

        if (!sendMessage) return;

        switch (gunType)
        {
            case GunTypeEnum.singleShot:
                MessagePopupController.Instance.PlayMessage("Single Shot");
                break;
            case GunTypeEnum.doubleShot:
                MessagePopupController.Instance.PlayMessage("Double Shot");
                break;
            case GunTypeEnum.tripleShot:
                MessagePopupController.Instance.PlayMessage("Triple Shot");
                break;
            case GunTypeEnum.quadShot:
                MessagePopupController.Instance.PlayMessage("Quad Shot");
                break;
            case GunTypeEnum.superTripleShot:
                MessagePopupController.Instance.PlayMessage("Super Triple Shot");
                break;
            case GunTypeEnum.fireShot:
                MessagePopupController.Instance.PlayMessage("Fire Shot");
                break;
            case GunTypeEnum.plasmaShot:
                MessagePopupController.Instance.PlayMessage("Plasma Shot");
                break;
            case GunTypeEnum.laserShot:
                MessagePopupController.Instance.PlayMessage("Laser Shot");
                break;
            default:
                MessagePopupController.Instance.PlayMessage("Error Happened");
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

    #endregion

    #region Handlers
    private void HandleMovement()
    {
        float horizontalInput = _playerInputHandler.MovementInput.x;
        float verticalInput = _playerInputHandler.MovementInput.y;

        if (_mirrorControls)
        {
            horizontalInput *= -1f;
            verticalInput *= -1f;
        }

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize();

        _rigidBody.velocity = movement * CurrentMoveSpeed;
    }

    private void HandleReload()
    {
        //If the magazine is not full and its time to reload, perform reload and reset the reload timer
        if (CurrentBullets < MaxBullets && _nextTimeToReload <= Time.timeSinceLevelLoad)
        {
            _nextTimeToReload = Time.timeSinceLevelLoad + (1f / ReloadRate);
            CurrentBullets++;
            _bulletChangeVoidEventChannelSO.RaiseEvent(CurrentBullets, MaxBullets);
        }

        //If the magazine is full also reset the reload timer, so the player cant instatly reload after shooting one bullet
        if (CurrentBullets == MaxBullets)
        {
            _nextTimeToReload = Time.timeSinceLevelLoad + (1f / ReloadRate);
        }
    }

    private void HandleShoot()
    {
        if (_playerInputHandler.ShootInput && _nextTimeToFire <= Time.timeSinceLevelLoad && CurrentBullets > 0)
        {
            _nextTimeToFire = Time.timeSinceLevelLoad + (1f / FireRate);
            CurrentBullets--;
            _bulletChangeVoidEventChannelSO.RaiseEvent(CurrentBullets, MaxBullets);

            switch (CurrentGunType)
            {
                case GunTypeEnum.singleShot:
                    HandlePlayerProjectileInstanciation();
                    break;
                case GunTypeEnum.doubleShot:
                    HandlePlayerProjectileInstanciation();
                    break;
                case GunTypeEnum.tripleShot:
                    HandlePlayerProjectileInstanciation();
                    break;
                case GunTypeEnum.quadShot:
                    HandlePlayerProjectileInstanciation();
                    break;
                case GunTypeEnum.superTripleShot:
                    HandlePlayerProjectileInstanciation();
                    break;
                case GunTypeEnum.fireShot:
                    HandlePlayerProjectileInstanciation();
                    break;
                case GunTypeEnum.plasmaShot:
                    HandlePlayerProjectileInstanciation();
                    break;
                case GunTypeEnum.laserShot:
                    HandlePlayerProjectileInstanciation();
                    break;
                default:
                    break;
            }
        }
    }

    private void HandlePauseGameInput()
    {
        if (_playerInputHandler.EscapeInput)
        {
            _gameUserInterfaceController.OpenPauseMenu();
        }
    }

    private void HandlePlayerProjectileInstanciation()
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
        else if (CurrentGunType == GunTypeEnum.tripleShot || CurrentGunType == GunTypeEnum.superTripleShot)
        {
            foreach (Transform child in playerProjectile.transform)
            {
                child.gameObject.SetActive(true);
                child.gameObject.transform.position = this.transform.position;
            }
        }
        else if (CurrentGunType == GunTypeEnum.fireShot)
        {
            for (int i = 0; i < playerProjectile.transform.childCount; i++)
            {
                playerProjectile.transform.GetChild(i).gameObject.SetActive(true);
                switch (i)
                {
                    case 0:
                        playerProjectile.transform.GetChild(i).gameObject.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
                        break;
                    case 1:
                        playerProjectile.transform.GetChild(i).gameObject.transform.position = new Vector2(this.transform.position.x - 0.5f, this.transform.position.y - 0.5f);
                        break;
                    case 2:
                        playerProjectile.transform.GetChild(i).gameObject.transform.position = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y - 0.5f);
                        break;
                    case 3:
                        playerProjectile.transform.GetChild(i).gameObject.transform.position = new Vector2(this.transform.position.x - 1f, this.transform.position.y - 1f);
                        break;
                    case 4:
                        playerProjectile.transform.GetChild(i).gameObject.transform.position = new Vector2(this.transform.position.x + 1f, this.transform.position.y - 1f);
                        break;
                    default:
                        break;
                }
            }
        }

    }

    internal void FreezePlayer()
    {
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    internal void UnfreezePlayer()
    {
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    #endregion

    #region Interface Implementation

    public void TakeDamage(int damageAmount)
    {
        StartCoroutine(DamageFlash());

        if (_hasShield)
        {
            RemoveShield();
            return;
        }

        CurrentHitPoints -= damageAmount;
        _healthChangeEventChannelSO.RaiseEvent(CurrentHitPoints);
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

        _healthChangeEventChannelSO.RaiseEvent(CurrentHitPoints);
    }

    public void TakeDamage(float damageAmount)
    {
        TakeDamage((int)damageAmount);
    }

    public void ProvideHealing(float healAmount)
    {
        ProvideHealing((int)healAmount);
    }

    #endregion

    #region Rendering
    private IEnumerator DamageFlash()
    {
        _shipSpriteRenderer.material = _damageFlashMaterial;
        yield return new WaitForSeconds(_damageFlashDuration);
        _shipSpriteRenderer.material = _defaultShipMaterial;
    }

    #endregion

    #region Buffs and Upgrades
    public void IncreaseSpeed(bool sendMessage) 
    {
        MaxMoveSpeed += 0.5f;
        CurrentMoveSpeed = MaxMoveSpeed;

        _movespeedChangeEventChannelSO.RaiseEvent(CurrentMoveSpeed);

        if (!sendMessage) return;
        MessagePopupController.Instance.PlayMessage(Constants.SPEED_UPGRADE_TEXT);
    }

    public void IncreaseBullet(bool sendMessage)
    {
        MaxBullets += 1;
        _bulletChangeVoidEventChannelSO.RaiseEvent(CurrentBullets, MaxBullets);

        if (!sendMessage) return;
        MessagePopupController.Instance.PlayMessage(Constants.BULLET_UPGRADE_TEXT);
    }

    private void UpgradeGun()
    {
        if (CurrentGunType == GunTypeEnum.laserShot) return;
        CurrentGunType++;
    }

    public void AddMoney(int money)
    {
        Money += money;
        PlayerPrefs.SetInt(Constants.MONEY_AMOUNT, Money);
        _moneyChangeEventChannelSO.RaiseEvent(Money);
    }

    public void RemoveMoney(int money)
    {
        Money -= money;
        PlayerPrefs.SetInt(Constants.MONEY_AMOUNT, Money);
        _moneyChangeEventChannelSO.RaiseEvent(Money);
    }

    public void AddShield()
    {
        _hasShield = true;
        _shieldSpriteRenderer.enabled = true;
        MessagePopupController.Instance.PlayMessage("Shield Activated");
    }

    public void RemoveShield()
    {
        _hasShield = false;
        _shieldSpriteRenderer.enabled = false;
    }

    #endregion

    #region Debuffs
    public void DebuffMovementSpeed()
    {
        CurrentMoveSpeed = MaxMoveSpeed * 0.75f;
        _movespeedChangeEventChannelSO.RaiseEvent(CurrentMoveSpeed);

        _thrusterAnimator.Play(Constants.THRUSTER_ANIMATION_SLOW);
        MessagePopupController.Instance.PlayMessage("Engine Failure");

        StartCoroutine(ResetMovementSpeed());
    }

    private IEnumerator ResetMovementSpeed()
    {
        yield return new WaitForSeconds(5f);
        CurrentMoveSpeed = MaxMoveSpeed;
        _movespeedChangeEventChannelSO.RaiseEvent(CurrentMoveSpeed);

        _thrusterAnimator.Play(Constants.THRUSTER_ANIMATION);
        MessagePopupController.Instance.PlayMessage("Engine Repaired");

    }

    public void DebuffMirrorControls()
    {
        _mirrorControls = true;
        MessagePopupController.Instance.PlayMessage("Mirror Controls");
        StartCoroutine(ResetMirrorControls());
    }

    private IEnumerator ResetMirrorControls()
    {
        yield return new WaitForSeconds(5f);
        MessagePopupController.Instance.PlayMessage("Controls Normalized");
        _mirrorControls = false;
    }

    #endregion

    #region Game Over
    private IEnumerator GameOver()
    {
        MessagePopupController.Instance.PlayMessage("Game Over");

        _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

        _deathExplosionOne.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        _deathExplosionTwo.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        _deathExplosionThree.SetActive(true);

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
    }

    #endregion
}
