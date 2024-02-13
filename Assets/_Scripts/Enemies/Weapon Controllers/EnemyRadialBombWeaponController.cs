using UnityEngine;

public class EnemyRadialBombWeaponController : BaseWeaponController
{
    [SerializeField] GameObject _enemyBombPrefab;

    [SerializeField] float _shotCooldown = 3f;

    private float _nextTimeToFire = 2f;

    private GameplayTimer _timer = GameplayTimer.Instance;

    public override void HandleShooting(Vector3 position, float fireRate)
    {
        if (_nextTimeToFire <= _timer.CurrentTime)
        {
            //Play shot audio

            _nextTimeToFire = _timer.CurrentTime + _shotCooldown;

            for (int i = 0; i < 18; i++)
            {
                GameObject radialEnemyBomb = Instantiate(_enemyBombPrefab, this.transform.position, Quaternion.Euler(0, 0, 20 * i));
                radialEnemyBomb.GetComponent<Rigidbody2D>().AddForce(radialEnemyBomb.transform.up * 150f, ForceMode2D.Force);
            }
        }
    }
}
