using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStatus playerStats;

    private CinemachineVirtualCamera followCamera;

    private Canvas[] inventory;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void RegisterPlayer(CharacterStatus player)
    {
        playerStats = player;
        followCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (followCamera != null)
        {
            followCamera.Follow = playerStats.transform.GetChild(2);
        }
        inventory = FindObjectsOfType<Canvas>();
        if (inventory != null)
        {
            foreach (var i in inventory)
            {
                Debug.Log("canvas:" + i.name);
            }
        }



    }

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var ovserver in endGameObservers)
        {
            ovserver.EndNotify();
        }
    }
}
