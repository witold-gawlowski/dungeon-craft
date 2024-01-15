using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalMapUIManager : MonoBehaviour
{
    private List<RunButtonScript> _runButtonScripts;

    public Action<RunConfig> runButtonPressedEvent;

    private Dictionary<RunButtonScript, RunConfig> _runConfigs;

    private void Awake()
    {
        _runButtonScripts = GetComponentsInChildren<RunButtonScript>().ToList();
        _runConfigs = new Dictionary<RunButtonScript, RunConfig>();
    }

    private void Start()
    {
        InitButtons();
        UpdateUI();

       GlobalMapManager.Instance.Subscribe(this);
    }

    private void OnDestroy()
    {
        GlobalMapManager.Instance.Unsubscribe(this);
    }

    public void UpdateUI()
    {
        foreach (var button in _runButtonScripts)
        {
            if (button)
            {   
                UpdateUI(button);
            }
        }
    }

    public void UpdateUI(RunButtonScript button)
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            button.ShowEditorVisuals();
            return;
        }

        if (!_runConfigs.ContainsKey(button))
        {
            button.SetHiddenVisuals();
            return;
        }

        var runConfig = _runConfigs[button];

        if (GlobalMapManager.Instance.IsRunCompleted(runConfig))
        {
            button.ShowConqueredVisuals();
            return;
        }

        var reward = runConfig.GetReward();
        bool blockOwned = GlobalBlockInventory.Instance.Contains(reward.BlockConfig);

        var selectedRun = GlobalMapManager.Instance.GetSelectedRunConfig();
        bool isButtonSelected = selectedRun == runConfig;

        if (blockOwned && isButtonSelected)
        {
            button.ShowSelectedDiscoveredVisuals(reward.BlockConfig);
            return;
        }

        if (blockOwned && !isButtonSelected)
        {
            button.ShowNotSelectedDisoveredVisuals(reward.BlockConfig);
            return;
        }

        if (!blockOwned && isButtonSelected)
        {
            button.ShowSelectedNotDiscoveredVisuals();
            return;
        }

        button.ShowNotSelectedNotDiscoveredVisuals();
    }

    public void UpdateEditorRunButtonVisuals()
    {
        var runButtons = GetComponentsInChildren<RunButtonScript>();
        foreach(var button in runButtons)
        {
            UpdateUI(button);
        }
    }   
   
    private void InitButtons()
    {
        foreach (var button in _runButtonScripts)
        {
            var seed = button.GetRunSeed();
            var config = button.GetRegionConfig();
            if (seed != 0 && config != null)
            {
                var newConfig = GlobalMapManager.Instance.GetRunConfig(seed, config);
                _runConfigs.Add(button, newConfig);

                button.runButtonPressedEvent += () => runButtonPressedEvent?.Invoke(newConfig);
            }
        }    
    }


    //TODO: remove when game works
    //private void HandleRunButtonPressed(int seed)
    //{
    //    runButtonPressedEvent?.Invoke(seed);
    //}
}
