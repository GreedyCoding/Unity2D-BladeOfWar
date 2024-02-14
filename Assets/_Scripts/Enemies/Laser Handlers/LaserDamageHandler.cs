using UnityEngine;

public class LaserDamageHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            other.GetComponent<PlayerController>().TakeDamage(1f);
        }
    }
}
