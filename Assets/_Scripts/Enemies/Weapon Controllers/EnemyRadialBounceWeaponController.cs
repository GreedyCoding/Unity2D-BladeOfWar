using UnityEngine;

public class EnemyRadialBounceWeaponController : BaseWeaponController
{
    [SerializeField] GameObject _enemyBounceProjectilePrefab;

    [SerializeField] float _shotCooldown = 3f;

    private float _nextTimeToFire = 2f;

    public override void HandleShooting(Vector3 position, float fireRate)
    {
        if (_nextTimeToFire <= Time.timeSinceLevelLoad)
        {
            AudioManager.Instance.PlayEnemyProjectileSound();

            _nextTimeToFire = Time.timeSinceLevelLoad + _shotCooldown;

            for (int i = 0; i < 6; i++)
            {
                GameObject radialEnemyBomb = Instantiate(_enemyBounceProjectilePrefab, position, Quaternion.Euler(0, 0, 60 * i));
            }
        }
    }

}
