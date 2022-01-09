using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemData_SO newItem, int amount)
    {
        bool existed = false;

        if (newItem.stackable)
        {
            foreach (var item in items)
            {
                if (item.itemData == newItem)
                {
                    item.amount += amount;
                    existed = true;
                    break;
                }
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemData == null && !existed)
            {
                items[i].itemData = newItem;
                items[i].amount = amount;
                break;
            }
        }
    }

    public void Test()
    {
        Debug.Log("单例is OK");
    }

}

[System.Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;

    public int amount;
}