using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class OutOfBoundsHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject collisionObject = other.gameObject;

        if(collisionObject.tag == "ProjectilePlayer" || collisionObject.tag == "ProjectileEnemy" || collisionObject.tag == "Enemy")
        {
            collisionObject.SetActive(false);
        } 
    }
}
