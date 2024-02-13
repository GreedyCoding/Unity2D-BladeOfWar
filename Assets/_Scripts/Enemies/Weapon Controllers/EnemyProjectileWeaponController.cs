using UnityEngine;

public class EnemyProjectileWeaponController : BaseWeaponController
{
    private float _nextTimeToFire = 2f;

    private GameplayTimer _timer = GameplayTimer.Instance;

    public override void HandleShooting(Vector3 position, float fireRate)
    {
        if (_nextTimeToFire <= _timer.CurrentTime)
        {
            //Play shot audio

            _nextTimeToFire = _timer.CurrentTime + 1 / fireRate;

            GameObject poolObject = ObjectPoolEnemyBombs.SharedInstance.GetPooledObject();
            poolObject.transform.position = position;
            poolObject.SetActive(true);
        }
    }
}

