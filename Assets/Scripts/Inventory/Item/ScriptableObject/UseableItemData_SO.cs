using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Useable Item", menuName = "Inventory/Usable Item Data")]
public class UseableItemData_SO : ScriptableObject
{
    public int healthPoint;

    public int magicPoint;

    public int energyPoint;

    public int attackPoint;

    public int defencePoint;
}
