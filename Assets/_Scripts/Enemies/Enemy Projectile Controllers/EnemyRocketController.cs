using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyRocketController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rocketDuration = 6f;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG);
        StartCoroutine(DestroyAfterTime(rocketDuration));
    }

    private void FixedUpdate()
    {
        FlyInDirectionOfTarget();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(1f);
            Destroy(this.gameObject);
        }
    }

    private void FlyInDirectionOfTarget()
    {
        Quaternion rotationPointingToTarget = Quaternion.LookRotation(Vector3.forward, transform.position - player.transform.position);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rotationPointingToTarget, 0.05f);
        rb.velocity = -(transform.up * moveSpeed);
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
