using System.Collections;
using UnityEngine;

public class EnemyLaserWeaponController : BaseWeaponController
{
    [SerializeField] SpriteRenderer _laserChargeupRenderer;
    [SerializeField] Animator _laserChargeupAnimator;

    [SerializeField] float _cooldown = 5f;

    private float _nextTimeToFire = 2f;

    private GameplayTimer _timer = GameplayTimer.Instance;

    public override void HandleShooting(Vector3 position, float fireRate)
    {
        if(_nextTimeToFire <= _timer.CurrentTime)
        {
            _nextTimeToFire = _timer.CurrentTime + _cooldown;

            _laserChargeupRenderer.enabled = true;
            _laserChargeupAnimator.enabled = true;
        }
    }
}
