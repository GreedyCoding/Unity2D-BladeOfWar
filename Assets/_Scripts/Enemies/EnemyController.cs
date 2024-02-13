using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{ 
    //Stats
    [SerializeField] EnemyStats _enemyStats;

    //Rendering
    [SerializeField] SpriteRenderer _enemySpriteRenderer;
    [SerializeField] Material _damageFlashMaterial;
    [SerializeField] Material _defaultShipMaterial;
    private float _damageFlashDuration = 0.1f;

    //Audio
    [SerializeField] AudioSource _audioSource;
    [SerializeField] List<AudioClip> _enemyBombSounds;
    
    //Object Components
    private Rigidbody2D _rigidbody;
    private BaseMovementController _movementController;
    private BaseWeaponController[] _weaponControllers;

    //Current Player Stats
    private float _currentHitPoints;

    //Movement
    private bool _reverseMovement;
    private float _wallBounceForce = 40f;

    //Loot Drop
    private bool _dropLoot;

    //Properties
    public float MaxHitPoints { get; private set; }
    public float MoveSpeed { get; private set; }
    public float FireRate { get; private set; }
    public float ProjectileSpeed { get; private set; }
    public EnemyTypeEnum CurrentEnemyType { get; private set; }


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _movementController = GetComponent<BaseMovementController>();
        _weaponControllers = GetComponents<BaseWeaponController>();

        SetStats();
    }

    private void OnEnable()
    {
        _enemySpriteRenderer.material = _defaultShipMaterial;
        SetStats();
    }

    private void Update()
    {
        HandleMovement();
        HandleShoot();
    }

    private void SetStats()
    {
        MaxHitPoints = _enemyStats.hitPoints;
        MoveSpeed = _enemyStats.moveSpeed;
        FireRate = _enemyStats.fireRate;
        ProjectileSpeed = _enemyStats.projectileSpeed;
        CurrentEnemyType = _enemyStats.enemyType;

        _currentHitPoints = MaxHitPoints;

        _dropLoot = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(1f);
        }
        else if (other.gameObject.CompareTag(Constants.WALL_LEFT_TAG))
        {
            if(_rigidbody != null)
                _rigidbody.AddForce(Vector2.right * _wallBounceForce, ForceMode2D.Impulse);

            _reverseMovement = !_reverseMovement;
        }
        else if (other.gameObject.CompareTag(Constants.WALL_RIGHT_TAG))
        {
            if (_rigidbody != null)
                _rigidbody.AddForce(Vector2.left * _wallBounceForce, ForceMode2D.Impulse);

            _reverseMovement = !_reverseMovement;

        }
    }

    private void HandleMovement()
    {
        _movementController.ApplyForceToRigidbody(_rigidbody, MoveSpeed, _reverseMovement);
    }

    private void HandleShoot()
    {
        if (_weaponControllers.Count() > 0)
        {
            for (int i = 0; i < _weaponControllers.Count(); i++)
            {
                _weaponControllers[i].HandleShooting(this.transform.position, FireRate);
            }
        }
        else
        {
            Debug.LogWarning("There is no WeaponController assigned to this enemy");
        }

    }

    public void TakeDamage(float damageAmount)
    {
        _currentHitPoints -= damageAmount;
        if (_currentHitPoints <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageFlash());
        }
    }

    private IEnumerator DamageFlash()
    {
        _enemySpriteRenderer.material = _damageFlashMaterial;
        yield return new WaitForSeconds(_damageFlashDuration);
        _enemySpriteRenderer.material = _defaultShipMaterial;
    }

    public void TakeDamage(int damageAmount)
    {
        TakeDamage((float)damageAmount);
    }

    private void Die()
    {
        AudioManager.Instance.PlayRandomShortExplosion();
        if (_dropLoot)
        {
            EnemyLootDropController.Instance.DropLoot(this.transform.position);
            //We need to set dropLoot to false here cause a second collision could happen before the gameObject is destroyed
            //which would trigger a TakeDamage call which could trigger Die() again spawning a second unwanted item
            _dropLoot = false;
        }
        this.gameObject.SetActive(false);
    }
}
