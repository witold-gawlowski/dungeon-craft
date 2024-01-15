using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RunReward
{
    public BlockConfig BlockConfig;
    public int Count;
}

public class RegionConfig : ScriptableObject
{
    [SerializeField] private List<MapPack> mapPacks;
    [SerializeField] private List<BlockConfig> rewardBlockConfigs;
    [SerializeField] private Color regionColor = Color.red;

    private RunConfig _configToFill;

    public Color GetRegionColor() {  return regionColor; }

    public void Fill(RunConfig config)
    {
        _configToFill = config;
        Random.InitState(config.GetSeed());

        SetMapConfigsByStage();
        SetStageCompleters();
        SetRunReward();
    }

    public virtual void SetMapConfigsByStage()
    {
        int stageCount = 1;
        var mapPacksByStage = Helpers.GetRandomSubset(mapPacks, stageCount);
        var mapByStage = mapPacksByStage.Select(mp => mp.Maps).ToList();
        _configToFill.SetMapsByStage(mapByStage);
    }

    public virtual void SetStageCompleters()
    {
        var result = new List<List<MapConfig>>();

        var stageCount = _configToFill.GetStageCount();
        for (int i = 0; i < stageCount; i++)
        {
            var stageMaps = _configToFill.GetMaps(i);
            var stageCompleter = Helpers.GetRandomElement(stageMaps);
            result.Add(new List<MapConfig>() {stageCompleter});
        }

        _configToFill.SetStageCompletersByStage(result);
    }

    public virtual void SetRunReward()
    {
        var config = Helpers.GetRandomElement(rewardBlockConfigs);
        var count = Random.Range(1, 5);
        var reward =  new RunReward { BlockConfig = config, Count = count };
        _configToFill.SetReward(reward);
    }
}
