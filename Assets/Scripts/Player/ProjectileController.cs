using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    ShipController shipController;
    Rigidbody2D rigidBody;


    void Start()
    {
        shipController = GameObject.Find("Player").GetComponent<ShipController>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidBody.AddForce(transform.up * shipController.projectileSpeed, ForceMode2D.Force);
    }
}
