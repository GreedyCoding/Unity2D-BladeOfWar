using UnityEngine;

public class EnemyProjectileWeaponController : BaseWeaponController
{
    private float _nextTimeToFire = 2f;

    private GameplayTimer _timer = GameplayTimer.Instance;

    public override void HandleShooting(Vector3 position, float fireRate)
    {
        if (_nextTimeToFire <= _timer.CurrentTime)
        {
            AudioManager.Instance.PlayEnemyProjectileSound();

            _nextTimeToFire = _timer.CurrentTime + 1 / fireRate;

            GameObject poolObject = ObjectPoolEnemyProjectiles.SharedInstance.GetPooledObject();
            poolObject.transform.position = position;
            poolObject.SetActive(true);
        }
    }
}

