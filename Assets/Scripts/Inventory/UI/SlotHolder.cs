using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotType { BAG, ACTION, WEAPON, EQUIPMENT }
public class SlotHolder : MonoBehaviour
{
    public SlotType slotType;

    public ItemUI itemUI;

    public void UpdateItemUI()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
            case SlotType.EQUIPMENT:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;

        }

        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetupItemUI(item.itemData, item.amount);
    }
}
