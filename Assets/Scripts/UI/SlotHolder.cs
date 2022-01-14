using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum SlotType { BAG, ACTION, WEAPON, EQUIPMENT }
public class SlotHolder : MonoBehaviour, IPointerClickHandler
{
    public SlotType slotType;

    public ItemUI itemUI;

    CharacterStatus characterStatus;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.Index].amount > 0)
        {
            //Debug.Log("blood: " + itemUI.GetItem().useableData.healthPoint);
            GameManager.Instance.playerStats.ChangeHealth(itemUI.GetItem().useableData.healthPoint);
            itemUI.Bag.items[itemUI.Index].amount -= 1;
        }
        if (itemUI.GetItem().itemType == ItemType.Weapon && itemUI.Bag.items[itemUI.Index].amount > 0)
        {

            GameManager.Instance.playerStats.EquipWeapon(itemUI.GetItem());
            itemUI.Bag.items[itemUI.Index].amount -= 1;
        }
        UpdateItemUI();
    }

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
