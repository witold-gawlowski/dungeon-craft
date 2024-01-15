using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MapPack", menuName = "ScriptableObjects/MapPack", order = 1)]
public class MapPack: ScriptableObject
{
    public List<MapConfig> Maps;
}
