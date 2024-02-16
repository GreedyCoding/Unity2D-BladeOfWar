using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class EnemyBombController : MonoBehaviour
{
    public float ProjectileDamage { get; private set; }

    void Start()
    {
        ProjectileDamage = 1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            other.GetComponent<PlayerController>().TakeDamage(ProjectileDamage);
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag(Constants.PROJECTILE_DESTROY_BOX))
        {
            Destroy(gameObject);
        }
    }
}
