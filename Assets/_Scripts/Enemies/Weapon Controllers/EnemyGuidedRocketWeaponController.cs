using UnityEngine;

public class EnemyGuidedRocketWeaponController : BaseWeaponController
{
    [SerializeField] GameObject _enemyGuidedRocketPrefab;

    [SerializeField] float _shotCooldown = 5f;

    private float _nextTimeToFire = 2f;

    public override void HandleShooting(Vector3 position, float fireRate)
    {
        if (_nextTimeToFire <= Time.timeSinceLevelLoad)
        {
            AudioManager.Instance.PlayEnemyRocketSound();

            _nextTimeToFire = Time.timeSinceLevelLoad + _shotCooldown;

            GameObject guidedRocket = Instantiate(_enemyGuidedRocketPrefab, position, Quaternion.identity);
        }
    }
}
