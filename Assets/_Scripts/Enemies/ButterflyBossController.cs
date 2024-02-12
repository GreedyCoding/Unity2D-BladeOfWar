using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButterflyBossController : MonoBehaviour, IDamageable
{
    //Enemy Stats
    [SerializeField] EnemyStats _enemyStats;
    [SerializeField] GameObject _enemyBombPrefab;
    [SerializeField] GameObject _guidedRocketPrefab;

    //Rendering
    [SerializeField] SpriteRenderer _enemySpriteRenderer;
    [SerializeField] Material _damageFlashMaterial;
    [SerializeField] Material _defaultShipMaterial;
    private float _damageFlashDuration = 0.1f;

    //Loot
    [SerializeField] GameObject _coinDropPrefab;

    //Event Scriptable Object
    [SerializeField] VoidEventChannelSO _bossDeathEventChannel;

    //Rigidbody
    private Rigidbody2D _rigidbody;

    //Timers
    private float _nextTimeToRadialShot = 0f;
    private float _nextTimeToGuidedRocket = 0f;

    //Cooldowns
    private float _radialShotCooldown = 2f;
    private float _guidedRocketCooldown = 4f;

    //Current Player Stats
    private float _currentHitPoints;
    private bool _dropLoot;

    //Wall Bounce
    private float _wallBounceForce = 40f;


    //Properties
    public float MaxHitPoints { get; private set; }
    public float MoveSpeed { get; private set; }
    public float ProjectileSpeed { get; private set; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        SetStats();
    }

    private void OnEnable()
    {
        _enemySpriteRenderer.material = _defaultShipMaterial;
    }

    private void FixedUpdate()
    {
        HandleAttackPattern();
        HandleMovement();
    }

    private void SetStats()
    {
        MaxHitPoints = _enemyStats.hitPoints;
        MoveSpeed = _enemyStats.moveSpeed;
        ProjectileSpeed = _enemyStats.projectileSpeed;

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
            if (_rigidbody != null)
                _rigidbody.AddForce(Vector2.right * _wallBounceForce, ForceMode2D.Impulse);
        }
        else if (other.gameObject.CompareTag(Constants.WALL_RIGHT_TAG))
        {
            if (_rigidbody != null)
                _rigidbody.AddForce(Vector2.left * _wallBounceForce, ForceMode2D.Impulse);
        }
    }

    private void HandleMovement()
    {
        float randomOffset = UnityEngine.Random.Range(0.2f, 1);
        Vector2 horizontalMovement = new Vector2(Mathf.Sin(Time.timeSinceLevelLoad), 0);

        _rigidbody.AddForce(horizontalMovement * randomOffset * 10f);

        if (_rigidbody.velocity.magnitude > MoveSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * MoveSpeed;
        }

        if(this.transform.position.y >= 1f && this.transform.position.y <= 2.5f)
        {
            _rigidbody.AddForce(Vector2.up * 10f, ForceMode2D.Force);
        }
    }

    private void HandleAttackPattern()
    {

        if (_nextTimeToRadialShot <= Time.timeSinceLevelLoad)
        {
            HandleRadialShot();
        }
        if (_nextTimeToGuidedRocket <= Time.timeSinceLevelLoad)
        {
            HandleGuidedRocketShot();
        }
    }


    private void HandleRadialShot()
    {
        _nextTimeToRadialShot = Time.timeSinceLevelLoad + _radialShotCooldown;
        for (int i = 0; i < 18; i++)
        {
            GameObject radialEnemyBomb = Instantiate(_enemyBombPrefab, this.transform.position, Quaternion.Euler(0, 0, 20 * i));
            radialEnemyBomb.GetComponent<Rigidbody2D>().AddForce(radialEnemyBomb.transform.up * 150f, ForceMode2D.Force);
        }
    }

    private void HandleGuidedRocketShot()
    {
        _nextTimeToGuidedRocket = Time.timeSinceLevelLoad + _guidedRocketCooldown;
        GameObject guidedRocket = Instantiate(_guidedRocketPrefab, this.transform.position, Quaternion.identity);
    }

    private void Die()
    {
        if(_dropLoot)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector3 lootPositionOffset = new Vector3(UnityEngine.Random.Range(-2.5f, 2.5f), UnityEngine.Random.Range(-2.5f, 2.5f), 0f);
                Vector3 lootPosition = this.transform.position + lootPositionOffset;

                GameObject coinDrop = Instantiate(_coinDropPrefab, lootPosition, Quaternion.identity);

                Rigidbody2D coinDropRigidBody;
                if (coinDrop.TryGetComponent<Rigidbody2D>(out coinDropRigidBody))
                {
                    coinDropRigidBody.gravityScale = 0.2f;
                }

            }

            //We need to set _dropLoot to false here cause a second collision could happen before the gameObject is destroyed
            //which would trigger a TakeDamage call which could trigger Die() again spawning a second unwanted item
            _dropLoot = false;
        }

        MessagePopupController.Instance.PlayMessage("Phase 1 Complete!");

        _bossDeathEventChannel.RaiseEvent();

        Destroy(this.gameObject);
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
}
