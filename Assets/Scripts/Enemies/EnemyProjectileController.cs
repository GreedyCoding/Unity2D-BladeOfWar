using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    EnemyController enemyController;
    Rigidbody2D rb;

    public float ProjectileDamage { get; private set; }
    public float ProjectileSpeed { get; private set; }

    void Start()
    {
        GetComponents();
        ProjectileDamage = 1f;
        ProjectileSpeed = 5f;
    }

    void Update()
    {
        rb.AddForce(Vector2.down * ProjectileSpeed, ForceMode2D.Force);
    }

    void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<ShipController>().TakeDamage(ProjectileDamage);
            Destroy(this.gameObject);
        }
    }
}
