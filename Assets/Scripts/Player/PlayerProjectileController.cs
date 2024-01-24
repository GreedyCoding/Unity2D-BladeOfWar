using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    PlayerController shipController;
    Rigidbody2D rb;

    public float ProjectileDamage { get; private set; }

    void Start()
    {
        GetComponents();
        SetProjectileDamage();
    }

    void Update()
    {
        rb.AddForce(transform.up * shipController.ProjectileSpeed, ForceMode2D.Force);
    }

    void GetComponents()
    {
        shipController = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(ProjectileDamage);

            if (this.gameObject.transform.parent == null)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    void SetProjectileDamage()
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
                ProjectileDamage = 7f;
                break;
            case GunTypeEnum.quadShot:
                ProjectileDamage = 7f;
                break;
            case GunTypeEnum.superTripleShot:
                ProjectileDamage = 14f;
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
