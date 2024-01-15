using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GlobalMapManager : MonoBehaviour
{
    public static GlobalMapManager Instance { get; private set; }

    private List<RunConfig> _runConfigs;
    private List<RunConfig> _completedRuns;
    private RunConfig _selectedRunConfig;
    private GlobalMapUIManager _ui;

    public Action datachanged;
    
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
        
        _completedRuns = new List<RunConfig>();
        _runConfigs = new List<RunConfig>();
    }

    public void Subscribe(GlobalMapUIManager ui)
    {
        ui.runButtonPressedEvent += HandleSeedSelected;
        _ui = ui;
    }

    public void Unsubscribe(GlobalMapUIManager ui)
    {
        ui.runButtonPressedEvent -= HandleSeedSelected;
        _ui = null;
    }

    public RunConfig GetRunConfig(int seed, RegionConfig config)
    {
        var existingConfig = Find(seed, config);
        if (existingConfig != null)
        {
            return existingConfig;
        }

        var newConfig = new RunConfig (seed, config );
        _runConfigs.Add(newConfig);
        return newConfig;
    }

    public RunConfig GetSelectedRunConfig()
    {
        return _selectedRunConfig;
    }

    public void HandleSeedSelected(RunConfig config)
    {
        _selectedRunConfig = config;
        _ui.UpdateUI();
        datachanged?.Invoke();
    }

    public void HandleRunCompleted()
    {
        AddCompletedRun(_selectedRunConfig);
    }

    public void AddCompletedRun(RunConfig run)
    {
        _completedRuns.Add(run);
        datachanged?.Invoke();
    }
    
    public bool IsRunCompleted(RunConfig run)
    {
        return _completedRuns.Contains(run);
    }

    public bool IsCurrentRunCompleted()
    {
        return IsRunCompleted(_selectedRunConfig);
    }

    private RunConfig Find(int Seed,  RegionConfig config)
    {
        foreach (var entry in _runConfigs)
        {
            if(entry.GetSeed() == Seed && entry.GetRegionConfig() == config)
            {
                return entry;
            }
        }
        return null;
    }
}
