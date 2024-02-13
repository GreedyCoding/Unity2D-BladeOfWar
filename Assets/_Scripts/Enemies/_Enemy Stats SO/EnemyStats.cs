using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyStats", menuName = "ScriptableObjects/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    public float hitPoints;

    public float moveSpeed;

    public float fireRate;
    public float projectileSpeed;

    public EnemyTypeEnum enemyType;
}

