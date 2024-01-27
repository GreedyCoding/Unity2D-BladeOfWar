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

    [Header("Damage Flash")]
    [SerializeField] SpriteRenderer shipSpriteRenderer;
    [SerializeField] Material damageFlashMaterial;
    [SerializeField] Material defaultShipMaterial;
    private float damageFlashDuration = 0.1f;

    [Header("Ship Stats")]
    [SerializeField] ShipStats shipStats;

    [Header("Shot Prefabs")]
    public GameObject singleShotPrefab;
    public GameObject doubleShotPrefab;
    public GameObject tripleShotPrefab;
    public GameObject quadShotPrefab;
    public GameObject superTripleShotPrefab;
    public GameObject fireShotPrefab;
    public GameObject plasmaShotPrefab;
    public GameObject laserShotPrefab;

    //Properties
    public int MaxHitPoints { get; private set; }
    public int CurrentHitPoints { get; private set; }
    public float MoveSpeed { get; private set; }
    public float CurrentMoveSpeed { get { return rb.velocity.magnitude; } }
    public int MaxBullets { get; private set; }
    public int CurrentBullets { get; private set; }
    public float FireRate { get; private set; }
    public float ProjectileSpeed { get; private set; }
    public float ReloadRate { get; private set; }
    public GunTypeEnum CurrentGunType { get; private set; }

    //Events
    public event EventHandler OnGunTypeChange;
    public event EventHandler OnHealthValueChange;
    public event EventHandler OnBulletValueChange;
 
    //Timers
    private float nextTimeToReload = 0f;
    private float nextTimeToFire = 0f;

    //Debuffs
    private bool mirrorControls = false;

    private void Start()
    {
        SetStats();
        SetGunType(shipStats.gunType, true);
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

        CurrentHitPoints = MaxHitPoints;
        OnHealthValueChange?.Invoke(this, EventArgs.Empty);

        CurrentBullets = MaxBullets;
        OnBulletValueChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetGunType(GunTypeEnum gunType, bool initialSet)
    {
        if(CurrentGunType == gunType && !initialSet)
        {
            IncreaseBullet();
            return;
        }
        CurrentGunType = gunType;
        OnGunTypeChange?.Invoke(this, EventArgs.Empty);
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

    //Game Over
    private void GameOver()
    {
        SceneManager.LoadScene("Game Scene");
    }

    //Interface Implementation
    public void TakeDamage(int damageAmount)
    {
        StartCoroutine(DamageFlash());
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
        OnHealthValueChange?.Invoke(this, EventArgs.Empty);
        if (CurrentHitPoints > MaxHitPoints)
        {
            CurrentHitPoints = MaxHitPoints;
        }
    }

    //Flash the ship white when taking damage
    private IEnumerator DamageFlash()
    {
        shipSpriteRenderer.material = damageFlashMaterial;
        yield return new WaitForSeconds(damageFlashDuration);
        shipSpriteRenderer.material = defaultShipMaterial;
    }

    //Functions to increase stats from Drops or ShopUpgrades
    public void IncreaseSpeed()
    {
        MoveSpeed += 0.5f;
    }

    public void IncreaseBullet()
    {
        MaxBullets += 1;
        OnBulletValueChange?.Invoke(this, EventArgs.Empty);
    }

    //Functions to debuff stats from MalusDrops
    public void DebuffMovementSpeed()
    {
        MoveSpeed = shipStats.moveSpeed * 0.75f;
        thrusterAnimator.Play("Thruster Animation Slow");
        StartCoroutine(ResetMovementSpeed());
    }

    private IEnumerator ResetMovementSpeed()
    {
        yield return new WaitForSeconds(5f);
        MoveSpeed = shipStats.moveSpeed;
        thrusterAnimator.Play("Thruster Animation");
    }

    public void DebuffMirrorControls()
    {
        mirrorControls = true;
        StartCoroutine(ResetMirrorControls());
    }

    private IEnumerator ResetMirrorControls()
    {
        yield return new WaitForSeconds(5f);
        mirrorControls = false;
    }

    public void ProvideHealing(float healAmount)
    {
        ProvideHealing((int)healAmount);
    }

    public void TakeDamage(float damageAmount)
    {
        TakeDamage((int)damageAmount);
    }
}
