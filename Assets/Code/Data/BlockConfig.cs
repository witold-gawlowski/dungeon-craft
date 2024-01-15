using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BlockConfig", order = 1)]
public class BlockConfig : ScriptableObject
{
    public GameObject prefab;
    public Sprite icon;
}