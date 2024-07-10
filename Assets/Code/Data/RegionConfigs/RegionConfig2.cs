using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "RegionConfig2", menuName = "ScriptableObjects/Once/RegionConfig2", order = 1)]
public class RegionConfig2 : RegionConfig
{
    public virtual void SetMapConfigsByStage()
    {
        int stageCount = 1;
        var mapPacksByStage = Helpers.GetRandomSubset(mapPacks, stageCount);
        var mapByStage = mapPacksByStage.Select(mp => mp.Maps).ToList();
        _configToFill.SetMapsByStage(mapByStage);
    }
}
