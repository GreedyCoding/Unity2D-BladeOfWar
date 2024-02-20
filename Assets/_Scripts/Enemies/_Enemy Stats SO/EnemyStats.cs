using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyStats", menuName = "ScriptableObjects/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    public float HitPoints;

    public float MoveSpeed;

    public float FireRate;
    public float ProjectileSpeed;

    public EnemyTypeEnum EnemyType;
}

