using UnityEngine;

public class EnemyBounceProjectileWeaponController : BaseWeaponController
{
    private float _nextTimeToFire = 2f;

    private GameplayTimer _timer = GameplayTimer.Instance;

    public override void HandleShooting(Vector3 position, float fireRate)
    {
        if (_nextTimeToFire <= _timer.CurrentTime)
        {
            //Play shot audio

            _nextTimeToFire = _timer.CurrentTime + 1f / fireRate;

            GameObject poolObject = ObjectPoolEnemyProjectiles.SharedInstance.GetPooledObject();
            poolObject.transform.position = position;
            poolObject.SetActive(true);

            foreach (Transform child in poolObject.transform)
            {
                child.position = position;
                child.gameObject.SetActive(true);
            }
        }
    }
}

