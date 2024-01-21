using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] PlayerInputHandler playerInputHandler;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] GameObject singleShotPrefab;
    [SerializeField] GameObject doubleShotPrefab;
    [SerializeField] GameObject tripleShotPrefab;
    [SerializeField] GameObject quadShotPrefab;
    [SerializeField] GameObject superTripleShotPrefab;
    [SerializeField] GameObject fireShotPrefab;
    [SerializeField] GameObject plasmaShotPrefab;
    [SerializeField] GameObject laserShotPrefab;

    [SerializeField] GunTypeEnum currentGunType = GunTypeEnum.singleShot;

    private float moveSpeed = 5f;

    [SerializeField] int currentBullets;
    private int maxBullets = 3;

    private float nextTimeToReload = 0f;
    private float reloadRate = 0.75f;

    private float nextTimeToFire = 0f;
    private float fireRate = 8f;

    public float projectileSpeed = 5f;

    private void Start()
    {
        currentBullets = maxBullets;
    }

    private void Update()
    {
        HandleMovement();
        HandleShoot();
        HandleReload();
    }

    private void HandleMovement()
    {
        float horizontalInput = playerInputHandler.movementInput.x;
        float verticalInput = playerInputHandler.movementInput.y;

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize(); 

        rb.velocity = movement * moveSpeed;
    }

    private void HandleReload()
    {
        //If the magazine is not full and its time to reload, perform reload and reset the reload timer
        if (currentBullets < maxBullets && nextTimeToReload <= Time.time)
        {
            nextTimeToReload = Time.time + (1f / reloadRate);
            currentBullets++;
        }

        //If the magazine is full also reset the reload timer, so the player cant instatly reload after shooting one bullet
        if (currentBullets == maxBullets)
        {
            nextTimeToReload = Time.time + (1f / reloadRate);
        }
    }

    private void HandleShoot()
    {
        if (playerInputHandler.shootInput && nextTimeToFire <= Time.time && currentBullets > 0)
        {
            nextTimeToFire = Time.time + (1f / fireRate);
            currentBullets--;

            switch (currentGunType)
            {
                case GunTypeEnum.singleShot:
                    Instantiate(singleShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.doubleShot:
                    Instantiate(doubleShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.tripleShot:
                    Instantiate(tripleShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.quadShot:
                    Instantiate(quadShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.superTripleShot:
                    Instantiate(superTripleShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.fireShot:
                    Instantiate(fireShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.plasmaShot:
                    Instantiate(plasmaShotPrefab, this.transform.position, this.transform.rotation);
                    break;
                case GunTypeEnum.laserShot:
                    Instantiate(laserShotPrefab, this.transform.position, this.transform.rotation);
                    break;
            }
        }
    }
}
