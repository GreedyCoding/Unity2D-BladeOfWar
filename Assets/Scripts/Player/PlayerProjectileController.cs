using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    PlayerController shipController;
    Rigidbody2D rb;

    public float ProjectileDamage { get; private set; }

    private void Start()
    {
        GetComponents();
        SetProjectileDamage();
    }

    private void Update()
    {
        rb.velocity = transform.up * shipController.ProjectileSpeed;
    }

    private void GetComponents()
    {
        shipController = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<IDamageable>().TakeDamage(ProjectileDamage);
            this.gameObject.SetActive(false);          
        }
        else if (other.gameObject.CompareTag("WallLeft") || other.gameObject.CompareTag("WallRight"))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void SetProjectileDamage()
    {
        switch (shipController.CurrentGunType)
        {
            case GunTypeEnum.singleShot:
                ProjectileDamage = 5f;
                break;
            case GunTypeEnum.doubleShot:
                ProjectileDamage = 6f;
                break;
            case GunTypeEnum.tripleShot:
                ProjectileDamage = 8f;
                break;
            case GunTypeEnum.quadShot:
                ProjectileDamage = 7f;
                break;
            case GunTypeEnum.superTripleShot:
                ProjectileDamage = 16f;
                break;
            case GunTypeEnum.fireShot:
                ProjectileDamage = 20f;
                break;
            case GunTypeEnum.plasmaShot:
                ProjectileDamage = 32f;
                break;
            case GunTypeEnum.laserShot:
                ProjectileDamage = 50f;
                break;
        }
    }
}
