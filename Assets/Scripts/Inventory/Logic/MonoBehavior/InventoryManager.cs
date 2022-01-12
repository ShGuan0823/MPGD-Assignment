using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalHolder;

        public RectTransform originalParent;
    }


    // TODO: 最后添加模版用于保存数据；
    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;

    public InventoryData_SO actionData;

    public InventoryData_SO equipmentData;

    [Header("Containers")]
    public ContainerUI inventoryUI;

    public ContainerUI actionUI;

    public ContainerUI equipmentUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;

    public DragData currentDrag;

    [Header("UI Panel")]
    public GameObject inventoryPanel;

    bool isOpen = false;

    private void Start()
    {
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    isOpen = !isOpen;
        //    inventoryPanel.gameObject.SetActive(isOpen);
        //}
    }

    #region check slot range
    public bool CheckInInventoryUI(Vector3 position)
    {
        for (int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            RectTransform rect = (RectTransform)inventoryUI.slotHolders[i].transform;

            if (RectTransformUtility.RectangleContainsScreenPoint(rect, position))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform rect = (RectTransform)actionUI.slotHolders[i].transform;

            if (RectTransformUtility.RectangleContainsScreenPoint(rect, position))
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
