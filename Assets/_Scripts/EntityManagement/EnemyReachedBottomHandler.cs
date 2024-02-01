using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReachedBottomHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject collisionObject = other.gameObject;

        if (collisionObject.tag == "Enemy")
        {
            collisionObject.transform.position = new Vector3(collisionObject.transform.position.x, 5.5f, collisionObject.transform.position.z);
        }
    }
}
