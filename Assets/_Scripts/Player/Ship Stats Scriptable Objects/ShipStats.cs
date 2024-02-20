using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShipStats", menuName = "ScriptableObjects/ShipStats", order = 1)]
public class ShipStats : ScriptableObject
{
    public int HitPoints;

    public float MoveSpeed;

    public int MaxBullets;
    public float FireRate;
    public float ReloadRate;
    public float ProjectileSpeed;

    public GunTypeEnum GunType;
}
