using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pick Up");
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itmeAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            Destroy(gameObject);
        }
    }
}
