using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CharacterStats { Idle, Walk, Run, Attack, Jump, Dead }
//public enum EnemyStats { Idle, Patrol, Chase, Attack, Guard, Dead }
public class CharacterStatus : MonoBehaviour
{
    public CharacterData_SO characterData;
    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;

    [Header("Weapon")]
    public Transform weaponSlot;

    #region Read from CharacterData_SO
    // 上限血量
    public int MaxHealth
    {
        get { if (characterData != null) { return characterData.maxHealth; } return 0; }

        set { characterData.maxHealth = value; }
    }
    // 当前血量
    public int CurrentHealth
    {
        get { if (characterData != null) { return characterData.currentHealth; } return 0; }

        set { characterData.currentHealth = value; }
    }
    // 基础护甲
    public int BaseDefence
    {
        get { if (characterData != null) { return characterData.baseDefence; } return 0; }

        set { characterData.baseDefence = value; }
    }
    // 当前护甲
    public int CurrentDefence
    {
        get { if (characterData != null) { return characterData.currentDefence; } return 0; }

        set { characterData.currentDefence = value; }
    }
    // 当前速度
    public float CurrentSpeed
    {
        get { if (characterData != null) { return characterData.currentSpeed; } return 0; }
        set { characterData.currentSpeed = value; }
    }
    // 当前能量
    public int CurrentEnergy
    {
        get { if (characterData != null) { return characterData.currentEnergy; } return 0; }
        set { characterData.currentEnergy = value; }
    }

    public int MaxEnergy
    {
        get { if (characterData != null) { return characterData.maxEnergy; } return 0; }
        set { characterData.maxEnergy = value; }
    }

    public CharacterStats PlayerStats
    {
        get { if (characterData != null) { return characterData.playerStats; } return 0; }
        set { characterData.playerStats = value; }
    }

    //public CharacterStats EnemyStats
    //{
    //    get { if (characterData != null) { return characterData.enemyStats; } return 0; }
    //    set { characterData.enemyStats = value; }
    //}

    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStatus attacker, CharacterStatus defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        Debug.Log("damage: " + damage);
    }

    private int CurrentDamage()
    {
        float coreDamage = Random.Range(attackData.minDamage, attackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMuliplier;
            Debug.Log("暴击! " + coreDamage);
        }

        return (int)coreDamage;
    }

    #endregion

    #region Equip Weapon
    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.weaponPrefab != null)
        {
            Instantiate(weapon.weaponPrefab, weaponSlot);
        }
    }

    #endregion

    #region Chage Data
    public void ChangeHealth(int amount)
    {
        if (CurrentHealth + amount <= MaxHealth)
            CurrentHealth += amount;
        else
            CurrentHealth = MaxHealth;
    }

    public void ChangeEnergy(int amount)
    {
        if (CurrentEnergy + amount <= MaxEnergy)
            CurrentEnergy += amount;
        else
            CurrentEnergy = MaxEnergy;
    }
    #endregion
}
