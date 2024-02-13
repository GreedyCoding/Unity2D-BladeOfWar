using UnityEngine;

public class EnemyRadialBombWeaponController : BaseWeaponController
{
    [SerializeField] GameObject _enemyBombPrefab;

    [SerializeField] float _shotCooldown = 3f;

    private float _nextTimeToFire = 2f;

    public override void HandleShooting(Vector3 position, float fireRate)
    {
        if (_nextTimeToFire <= Time.timeSinceLevelLoad)
        {
            //Play shot audio

            _nextTimeToFire = Time.timeSinceLevelLoad + _shotCooldown;

            for (int i = 0; i < 18; i++)
            {
                GameObject radialEnemyBomb = Instantiate(_enemyBombPrefab, position, Quaternion.Euler(0, 0, 20 * i));
                radialEnemyBomb.GetComponent<Rigidbody2D>().AddForce(radialEnemyBomb.transform.up * 150f, ForceMode2D.Force);
            }
        }
    }
}
