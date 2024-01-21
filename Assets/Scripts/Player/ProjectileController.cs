using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    ShipController shipController;
    Rigidbody2D rigidBody;

    public float ProjectileDamage { get; private set; }


    void Start()
    {
        GetComponents();
        SetProjectileDamage();
    }

    void Update()
    {
        rigidBody.AddForce(transform.up * shipController.ProjectileSpeed, ForceMode2D.Force);
    }

    void GetComponents()
    {
        shipController = GameObject.Find("Player").GetComponent<ShipController>();
        rigidBody = GetComponent<Rigidbody2D>();
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
