using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Test Status/Player Data")]
public class TestPlayerStatus : ScriptableObject
{
    [Header("Status Info")]

    public int maxHealth;

    public int currentHealth;

    public int baseDefence;

    public int currentDefence;

    public float currentSpeed;

    public int hungry;
}
