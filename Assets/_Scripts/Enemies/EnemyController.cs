using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyStats _enemyStats;

    [SerializeField] GameObject _bonusDropPrefab;
    [SerializeField] GameObject _malusDropPrefab;
    [SerializeField] GameObject _coinDropPrefab;

    [SerializeField] SpriteRenderer _enemySpriteRenderer;
    [SerializeField] Material _damageFlashMaterial;
    [SerializeField] Material _defaultShipMaterial;
    private float _damageFlashDuration = 0.1f;

    [SerializeField] AudioSource _audioSource;
    [SerializeField] List<AudioClip> _enemyBombSounds;

    private Rigidbody2D _rigidbody;

    public float MaxHitPoints { get; private set; }
    public float MoveSpeed { get; private set; }
    public float FireRate { get; private set; }
    public float ProjectileSpeed { get; private set; }
    public EnemyTypeEnum CurrentEnemyType { get; private set; }

    //Timers
    private float _nextTimeToFire = 2f;

    //Current Player Stats
    private float _currentHitPoints;

    //Movement
    private bool _reverseMovement;
    private float _wallBounceForce = 40f;
    private float _randomMovementSpeedOffset;
    private float _randomSinusOffset;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        SetStats();
    }

    private void OnEnable()
    {
        _enemySpriteRenderer.material = _defaultShipMaterial;
        SetStats();
    }

    private void Update()
    {
        HandleShoot();
        HandleMovement();
    }

    private void SetStats()
    {
        MaxHitPoints = _enemyStats.hitPoints;
        MoveSpeed = _enemyStats.moveSpeed;
        FireRate = _enemyStats.fireRate;
        ProjectileSpeed = _enemyStats.projectileSpeed;
        CurrentEnemyType = _enemyStats.enemyType;

        _currentHitPoints = MaxHitPoints;

        _randomMovementSpeedOffset = Random.Range(0f, 10f);
        _randomSinusOffset = Random.Range(0f, 2f);
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
        Vector2 horizontalMovement = new Vector2(Mathf.Sin(Time.timeSinceLevelLoad) * _randomSinusOffset, 0);
        if(_reverseMovement)
        {
            horizontalMovement *= -1;
        }

        _rigidbody.AddForce(horizontalMovement * _randomMovementSpeedOffset);

        if (_rigidbody.velocity.magnitude > MoveSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * MoveSpeed;
        }
    }

    private void HandleShoot()
    {
        if(_nextTimeToFire <= Time.timeSinceLevelLoad)
        {
            _nextTimeToFire = Time.timeSinceLevelLoad + 1f / FireRate;
            
            int randomAudioIndex = Random.Range(0, _enemyBombSounds.Count);
            _audioSource.clip = _enemyBombSounds[randomAudioIndex];
            _audioSource.Play();

            switch (CurrentEnemyType)
            {
                case EnemyTypeEnum.beetle:
                    GameObject poolObject = ObjectPoolEnemyBombs.SharedInstance.GetPooledObject();
                    poolObject.transform.position = this.transform.position;
                    poolObject.SetActive(true);
                    break;
                case EnemyTypeEnum.dragonfly:
                    GameObject poolObject2 = ObjectPoolEnemyBombs.SharedInstance.GetPooledObject();
                    poolObject2.transform.position = this.transform.position;
                    poolObject2.SetActive(true);
                    break;
                case EnemyTypeEnum.butterfly:
                    GameObject poolObject3 = ObjectPoolEnemyBombs.SharedInstance.GetPooledObject();
                    poolObject3.transform.position = this.transform.position;
                    poolObject3.SetActive(true);
                    break;
                case EnemyTypeEnum.smallShip:
                    GameObject poolObject4 = ObjectPoolEnemyBombs.SharedInstance.GetPooledObject();
                    poolObject4.transform.position = this.transform.position;
                    poolObject4.SetActive(true);
                    break;
                case EnemyTypeEnum.bigShip:
                    GameObject poolObject5 = ObjectPoolEnemyBombs.SharedInstance.GetPooledObject();
                    poolObject5.transform.position = this.transform.position;
                    poolObject5.SetActive(true);
                    break;
                case EnemyTypeEnum.baboBoss:
                    GameObject poolObject6 = ObjectPoolEnemyBombs.SharedInstance.GetPooledObject();
                    poolObject6.transform.position = this.transform.position;
                    poolObject6.SetActive(true);
                    break;
                default:
                    break;
            }
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

    private void RollForLoot()
    {
        float bonusDropChance = 0.33f;
        float coinDropChance = 0.33f;
        float malusDropChance = 0.33f;

        float bonusDropRange = bonusDropChance;
        float coinDropRange = coinDropChance + bonusDropChance;
        float malusDropRange = malusDropChance + bonusDropChance + coinDropChance;
        
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber <= bonusDropRange)
        {
            Instantiate(_bonusDropPrefab, this.transform.position, Quaternion.identity);
            return;
        }
        else if (randomNumber > bonusDropRange && randomNumber <= coinDropRange)
        {
            Instantiate(_coinDropPrefab, this.transform.position, Quaternion.identity);
            return;
        }
        else if (randomNumber > coinDropRange && randomNumber <= malusDropRange)
        {
            Instantiate(_malusDropPrefab, this.transform.position, Quaternion.identity);
            return;
        }
    }

    private void Die()
    {
        AudioManager.Instance.PlayRandomShortExplosion();
        RollForLoot();
        this.gameObject.SetActive(false);
    }
}
