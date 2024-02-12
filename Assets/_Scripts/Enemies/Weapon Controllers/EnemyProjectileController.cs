using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    public float ProjectileDamage { get; private set; }
    public float ProjectileSpeed { get; private set; }

    void Start()
    {
        GetComponents();
        ProjectileDamage = 1f;
        ProjectileSpeed = 7f;
    }

    void Update()
    {
        _rigidbody.velocity = (Vector2.down * ProjectileSpeed);
    }

    void GetComponents()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            other.GetComponent<PlayerController>().TakeDamage(ProjectileDamage);
            this.gameObject.SetActive(false);
        }
    }
}
