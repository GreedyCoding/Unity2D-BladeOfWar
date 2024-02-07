using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButterflyBossController : MonoBehaviour, IDamageable
{
    //Enemy Stats
    [SerializeField] EnemyStats enemyStats;
    [SerializeField] GameObject enemyBombPrefab;
    [SerializeField] GameObject guidedRocketPrefab;

    //Rendering
    [SerializeField] SpriteRenderer enemySpriteRenderer;
    [SerializeField] Material damageFlashMaterial;
    [SerializeField] Material defaultShipMaterial;
    private float damageFlashDuration = 0.1f;

    //Loot
    [SerializeField] GameObject coinDropPrefab;

    //Rigidbody
    private Rigidbody2D rb;

    //Timers
    private float nextTimeToRadialShot = 0f;
    private float nextTimeToGuidedRocket = 0f;

    //Cooldowns
    private float radialShotCooldown = 2f;
    private float guidedRocketCooldown = 4f;

    //Properties
    public float MaxHitPoints { get; private set; }
    public float MoveSpeed { get; private set; }
    public float ProjectileSpeed { get; private set; }

    //Event Scriptable Object
    [SerializeField] VoidEventChannelSO _bossDeathEventChannel;

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
    }

    private void FixedUpdate()
    {
        HandleAttackPattern();
        HandleMovement();
    }

    private void SetStats()
    {
        MaxHitPoints = enemyStats.hitPoints;
        MoveSpeed = enemyStats.moveSpeed;
        ProjectileSpeed = enemyStats.projectileSpeed;

        currentHitPoints = MaxHitPoints;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(1f);
        }
        else if (other.gameObject.CompareTag(Constants.WALL_LEFT_TAG))
        {
            if (rb != null)
                rb.AddForce(Vector2.right * wallBounceForce, ForceMode2D.Impulse);
        }
        else if (other.gameObject.CompareTag(Constants.WALL_RIGHT_TAG))
        {
            if (rb != null)
                rb.AddForce(Vector2.left * wallBounceForce, ForceMode2D.Impulse);
        }
    }

    private void HandleMovement()
    {
        float randomOffset = UnityEngine.Random.Range(0.2f, 1);
        Vector2 horizontalMovement = new Vector2(Mathf.Sin(Time.timeSinceLevelLoad), 0);

        rb.AddForce(horizontalMovement * randomOffset * 10f);

        if (rb.velocity.magnitude > MoveSpeed)
        {
            rb.velocity = rb.velocity.normalized * MoveSpeed;
        }

        if(this.transform.position.y >= 1f && this.transform.position.y <= 2.5f)
        {
            rb.AddForce(Vector2.up * 10f, ForceMode2D.Force);
        }
    }

    private void HandleAttackPattern()
    {

        if (nextTimeToRadialShot <= Time.timeSinceLevelLoad)
        {
            HandleRadialShot();
        }
        if (nextTimeToGuidedRocket <= Time.timeSinceLevelLoad)
        {
            HandleGuidedRocketShot();
        }
    }


    private void HandleRadialShot()
    {
        nextTimeToRadialShot = Time.timeSinceLevelLoad + radialShotCooldown;
        for (int i = 0; i < 18; i++)
        {
            GameObject radialEnemyBomb = Instantiate(enemyBombPrefab, this.transform.position, Quaternion.Euler(0, 0, 20 * i));
            radialEnemyBomb.GetComponent<Rigidbody2D>().AddForce(radialEnemyBomb.transform.up * 150f, ForceMode2D.Force);
        }
    }

    private void HandleGuidedRocketShot()
    {
        nextTimeToGuidedRocket = Time.timeSinceLevelLoad + guidedRocketCooldown;
        GameObject guidedRocket = Instantiate(guidedRocketPrefab, this.transform.position, Quaternion.identity);
    }

    private void Die()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 lootPositionOffset = new Vector3(UnityEngine.Random.Range(-2.5f, 2.5f), UnityEngine.Random.Range(-2.5f, 2.5f), 0f);
            Vector3 lootPosition = this.transform.position + lootPositionOffset;
            Instantiate(coinDropPrefab, lootPosition, Quaternion.identity);
        }

        MessagePopupController.Instance.PlayMessage("Phase 1 Complete!");

        _bossDeathEventChannel.RaiseEvent();

        Destroy(this.gameObject);
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
}
