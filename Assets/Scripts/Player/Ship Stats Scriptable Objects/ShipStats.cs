using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShipStats", menuName = "ScriptableObjects/ShipStats", order = 1)]
public class ShipStats : ScriptableObject
{
    public int hitPoints;

    public float moveSpeed;

    public int maxBullets;
    public float fireRate;
    public float reloadRate;
    public float projectileSpeed;

    public GunTypeEnum gunType;
}
