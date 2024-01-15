using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapGeneratorConfig", menuName = "ScriptableObjects/MapGeneratorConfig", order = 1)]
public class MapGeneratorConfig : ScriptableObject
{
    public int width;
    public int height;
    public float initial;
    public int birthLimit;
    public int deathLimit;
    public int steps;
}
