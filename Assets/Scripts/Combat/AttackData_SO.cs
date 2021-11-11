using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;

    public float skillRange;

    public float coolDown;

    public int minDamage;

    public int maxDamage;
    // 暴击伤害加成
    public float criticalMuliplier;
    // 暴击率
    public float criticalChance;
}
