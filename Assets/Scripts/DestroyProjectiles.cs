using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class DestroyProjectiles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "ProjectilePlayer" || other.gameObject.tag == "ProjectileEnemy")
        {
            if(other.gameObject.transform.parent != null)
            {
                Destroy(other.gameObject.transform.parent.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}
