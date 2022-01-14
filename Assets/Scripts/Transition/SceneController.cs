using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public GameObject playerPrefab;
    GameObject player;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void TransitionToDesitination(TransitionPoint transitionPoint)
    {

        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.desitiantionTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.desitiantionTag));
                break;
        }
    }

    IEnumerator Transition(string sceneName, TransitionDesitination.DesitiantionTag desitinationTag)
    {
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance.SaveData();
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDesitination(desitinationTag).transform.position, GetDesitination(desitinationTag).transform.rotation);
            SaveManager.Instance.LoadPlayerData();
            //SaveManager.Instance.LoadBagData();
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStats.gameObject;
            player.transform.SetPositionAndRotation(GetDesitination(desitinationTag).transform.position, GetDesitination(desitinationTag).transform.rotation);
            yield return null;
        }

    }

    private TransitionDesitination GetDesitination(TransitionDesitination.DesitiantionTag desitinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDesitination>();

        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].desitinationTag == desitinationTag)
                return entrances[i];
        }
        return null;
    }
}
