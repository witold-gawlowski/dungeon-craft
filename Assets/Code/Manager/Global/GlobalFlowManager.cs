using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalFlowManager : MonoBehaviour
{
    public static GlobalFlowManager Instance { get; private set; }

    private RunConfig _loadedRunConfig;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void HandleRunStart()
    {
        var selectedRunConfig = GlobalMapManager.Instance.GetSelectedRunConfig();
        if(selectedRunConfig != _loadedRunConfig)
        {
            DestroyMissionManagers();
        }

        SceneManager.LoadScene("2-LevelShop");
        _loadedRunConfig = selectedRunConfig;
    }

    private void DestroyMissionManagers()
    {
        var missionManagers = GameObject.FindGameObjectsWithTag("MissionManager");
        foreach (var missionManager in missionManagers)
        {
            Debug.Log("destroying " + missionManager.name);
            Destroy(missionManager);
        }
    }

}
