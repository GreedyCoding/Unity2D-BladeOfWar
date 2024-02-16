using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounceProjectileController : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    public float ProjectileDamage { get; private set; }
    public float ProjectileSpeed { get; private set; }

    private void Awake()
    {
        ProjectileDamage = 1f;
        ProjectileSpeed = 7f;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _rigidbody.velocity = (this.transform.up * -1f * ProjectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Constants.WALL_LEFT_TAG) || other.gameObject.CompareTag(Constants.WALL_RIGHT_TAG))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x * -1, _rigidbody.velocity.y);
        }
        else if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            other.GetComponent<PlayerController>().TakeDamage(ProjectileDamage);
            this.gameObject.SetActive(false);
        }
        
    }
}
