using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyStats enemyStats;

    [SerializeField] GameObject bonusDropPrefab;
    [SerializeField] GameObject malusDropPrefab;
    [SerializeField] GameObject coinDropPrefab;

    [SerializeField] SpriteRenderer enemySpriteRenderer;
    [SerializeField] Material damageFlashMaterial;
    [SerializeField] Material defaultShipMaterial;
    private float damageFlashDuration = 0.1f;

    private Rigidbody2D rb;

    public float MaxHitPoints { get; private set; }
    public float MoveSpeed { get; private set; }
    public float FireRate { get; private set; }
    public float ProjectileSpeed { get; private set; }
    public EnemyTypeEnum CurrentEnemyType { get; private set; }

    //Timers
    private float nextTimeToFire = 2f;

    //Current Player Stats
    private float currentHitPoints;

    //Wall Bounce
    private float wallBounceForce = 40f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetStats();
    }

    private void OnEnable()
    {
        enemySpriteRenderer.material = defaultShipMaterial;
        SetStats();
    }

    private void Update()
    {
        HandleShoot();
        HandleMovement();
    }

    private void SetStats()
    {
        MaxHitPoints = enemyStats.hitPoints;
        MoveSpeed = enemyStats.moveSpeed;
        FireRate = enemyStats.fireRate;
        ProjectileSpeed = enemyStats.projectileSpeed;
        CurrentEnemyType = enemyStats.enemyType;

        currentHitPoints = MaxHitPoints;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(1f);
        }
        else if (other.gameObject.CompareTag("WallLeft"))
        {
            if(rb != null)
                rb.AddForce(Vector2.right * wallBounceForce, ForceMode2D.Impulse);
        }
        else if (other.gameObject.CompareTag("WallRight"))
        {
            if (rb != null)
                rb.AddForce(Vector2.left * wallBounceForce, ForceMode2D.Impulse);
        }
    }

    private void HandleMovement()
    {
        float randomOffset = Random.Range(0.2f, 1);
        Vector2 horizontalMovement = new Vector2(Mathf.Sin(Time.timeSinceLevelLoad), 0);

        rb.AddForce(horizontalMovement * randomOffset);

        if (rb.velocity.magnitude > MoveSpeed)
        {
            rb.velocity = rb.velocity.normalized * MoveSpeed;
        }
    }

    private void HandleShoot()
    {
        if(nextTimeToFire <= Time.timeSinceLevelLoad)
        {
            nextTimeToFire = Time.timeSinceLevelLoad + 1f / FireRate;
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
                default:
                    break;
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHitPoints -= damageAmount;
        if (currentHitPoints <= 0)
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
        enemySpriteRenderer.material = damageFlashMaterial;
        yield return new WaitForSeconds(damageFlashDuration);
        enemySpriteRenderer.material = defaultShipMaterial;
    }

    public void TakeDamage(int damageAmount)
    {
        TakeDamage((float)damageAmount);
    }

    private void Die()
    {
        RollForLoot();
        this.gameObject.SetActive(false);
    }

    private void RollForLoot()
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber <= 0.1f)
        {
            Instantiate(bonusDropPrefab, this.transform.position, Quaternion.identity);
        }
        else if (randomNumber <= 0.2f)
        {
            Instantiate(coinDropPrefab, this.transform.position, Quaternion.identity);
        }
        else if (randomNumber <= 0.3f)
        {
            Instantiate(malusDropPrefab, this.transform.position, Quaternion.identity);
        }
    }
}
