using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class OffscreenObjectHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject collisionObject = other.gameObject;

        if(collisionObject.tag == "ProjectilePlayer" || collisionObject.tag == "ProjectileEnemy")
        {
            if(collisionObject.transform.parent != null)
            {
                collisionObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                collisionObject.SetActive(false);
            }
        } 
        else if (collisionObject.tag == "Enemy")
        {
            collisionObject.transform.position = new Vector3(collisionObject.transform.position.x, 5.5f, collisionObject.transform.position.z);
        }
    }
}
