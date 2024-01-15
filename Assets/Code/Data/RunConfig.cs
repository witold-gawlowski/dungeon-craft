using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class RunConfig
{
    private int _seed;
    private RegionConfig _regionConfig;

    private List<List<MapConfig>> _mapsByStage;
    private List<List<MapConfig>> _stageCompletersByStage;
    private RunReward _reward;

    public RunConfig(int seed, RegionConfig regionConfig)
    {
        _seed = seed;
        _regionConfig = regionConfig;
        Init();
    }

    void Init()
    {
        _regionConfig.Fill(this);
    }

    public bool IsValid()
    {
        return _seed != 0 && _regionConfig != null;
    }

    public int GetSeed()
    {
        return _seed;
    }

    public RegionConfig GetRegionConfig()
    {
        return _regionConfig;
    }

    public virtual int GetStageCount()
    {
        return _mapsByStage.Count;
    }

    public List<MapConfig> GetMaps(int stage)
    {
        return _mapsByStage[stage];
    }

    public List<MapConfig> GetStageCompleters(int stage)
    {
        return _stageCompletersByStage[stage];
    }

    public RunReward GetReward()
    {
        return _reward;
    }

    public int GetShopSeed()
    {
        return 123;
    }

    public void SetReward(RunReward reward)
    {
        _reward = reward;
    }

    public void SetMapsByStage(List<List<MapConfig>> mapsByStage)
    {
        _mapsByStage = mapsByStage;
    }

    public void SetStageCompletersByStage(List<List<MapConfig>> stageCompletersBystage)
    {
        _stageCompletersByStage = stageCompletersBystage;
    }
}
