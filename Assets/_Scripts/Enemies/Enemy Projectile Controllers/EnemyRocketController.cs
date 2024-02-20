using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyRocketController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _rocketDuration = 6f;

    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG);
        StartCoroutine(DestroyAfterTime(_rocketDuration));
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
        Quaternion rotationPointingToTarget = Quaternion.LookRotation(Vector3.forward, transform.position - _player.transform.position);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rotationPointingToTarget, 0.05f);
        _rigidbody.velocity = -(transform.up * _moveSpeed);
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
