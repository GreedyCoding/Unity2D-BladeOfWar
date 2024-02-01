using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
