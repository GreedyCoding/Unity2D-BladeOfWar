using UnityEngine;

public abstract class BaseWeaponController : MonoBehaviour
{
    public abstract void HandleShooting(Vector3 position, float fireRate);
}
