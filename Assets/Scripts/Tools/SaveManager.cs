using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    MyInputAction input;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        input = new MyInputAction();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Test.Save.performed += Save_performed;
        input.Test.Load.performed += Load_performed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Test.Save.performed += Save_performed;
        input.Test.Load.performed += Load_performed;
    }

    public void Load_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        LoadPlayerData();
    }

    public void Save_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        SavePlayerData();
    }

    private void Update()
    {

    }

    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }

    public void SaveBagData()
    {
        Debug.Log("save");
        Save(InventoryManager.Instance.inventoryData, InventoryManager.Instance.inventoryData.name);
    }

    public void LoadBagData()
    {
        Debug.Log("load");
        Load(InventoryManager.Instance.inventoryData, InventoryManager.Instance.inventoryData.name);
    }

    public void Save(Object data, string key)
    {
        // pretty
        var jsonData = JsonUtility.ToJson(data, true); ;
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }

    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
