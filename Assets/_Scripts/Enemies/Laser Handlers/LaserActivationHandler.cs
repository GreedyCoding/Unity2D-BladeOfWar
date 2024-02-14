using System.Collections;
using UnityEngine;

public class LaserActivationHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer _laserChargeupRenderer;
    [SerializeField] Animator _laserChargeupAnimator;

    [SerializeField] SpriteRenderer _laserBeamRenderer;
    [SerializeField] Animator _laserBeamAnimator;
    [SerializeField] BoxCollider2D _laserBeamCollider;

    [SerializeField] float _duration = 1f;

    public void SetLaserBeamToActive()
    {
        _laserChargeupRenderer.enabled = false;
        _laserChargeupAnimator.enabled = false;

        _laserBeamRenderer.enabled = true;
        _laserBeamAnimator.enabled = true;
        _laserBeamCollider.enabled = true;

        StartCoroutine(DisableLaserBeam(_duration));
    }

    private IEnumerator DisableLaserBeam(float duration)
    {
        yield return new WaitForSeconds(duration);
        _laserBeamRenderer.enabled = false;
        _laserBeamAnimator.enabled = false;
        _laserBeamCollider.enabled = false;
    }
}
