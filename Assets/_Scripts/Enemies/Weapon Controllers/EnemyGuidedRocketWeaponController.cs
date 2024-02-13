using UnityEngine;

public class EnemyGuidedRocketWeaponController : BaseWeaponController
{
    [SerializeField] GameObject _enemyGuidedRocketPrefab;

    [SerializeField] float _shotCooldown = 5f;

    private float _nextTimeToFire = 2f;

    private GameplayTimer _timer = GameplayTimer.Instance;

    public override void HandleShooting(Vector3 position, float fireRate)
    {
        if (_nextTimeToFire <= _timer.CurrentTime)
        {
            //Play shot audio

            _nextTimeToFire = _timer.CurrentTime + _shotCooldown;

            GameObject guidedRocket = Instantiate(_enemyGuidedRocketPrefab, this.transform.position, Quaternion.identity);
        }
    }
}
