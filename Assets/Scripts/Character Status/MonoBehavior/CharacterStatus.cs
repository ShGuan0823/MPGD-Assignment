using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStatus : MonoBehaviour
{
    public CharacterData_SO characterData;
    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;

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
    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStatus attacker, CharacterStatus defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        //TODO: Update UI
        //TODO: Update exp
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
}
