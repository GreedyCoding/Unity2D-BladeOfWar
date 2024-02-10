using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class OutOfBoundsHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject collisionObject = other.gameObject;

        if(collisionObject.CompareTag(Constants.PLAYER_PROJECTILE_TAG) || collisionObject.tag == Constants.ENEMY_PROJECTILE_TAG || collisionObject.tag == Constants.ENEMY_TAG)
        {
            collisionObject.SetActive(false);
        }

        if (collisionObject.CompareTag(Constants.ITEM_DROP_TAG))
        {
            Destroy(collisionObject);
        }
    }
}
