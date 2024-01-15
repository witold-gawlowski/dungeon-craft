using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    private static MapManager instance; 
    private Dictionary<Vector2Int, GameObject> levelMap;
    private HashSet<Vector2Int> blockedCoords;
    private HashSet<GameObject> placedBlocks;

    // Start is called before the first frame update
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        levelMap = new Dictionary<Vector2Int, GameObject>();
        blockedCoords = new HashSet<Vector2Int>();
        placedBlocks = new HashSet<GameObject>();
        
        LoadSelectedMap();
    }

    private void Start()
    {
        var levelTiles = GameObject.FindGameObjectsWithTag("LevelTile");
        foreach (var tile in levelTiles)
        {
            levelMap.TryAdd(Helpers.RoundPosition(tile.transform.position), tile);
        }
    }

    public int GetCoveredArea()
    {
        int result = 0;
        foreach (var block in placedBlocks)
        {
            foreach (Transform t in block.transform)
            {
                if (t != block.transform && t.CompareTag("BlockTile"))
                {
                    result++;
                }
            }
        }
        return result;
    }
    
    public static MapManager GetInstance()
    {
        return instance;
    }
    
    public void Place(GameObject block, Vector2Int coordinates)
    {
        foreach (Vector2Int tileCoords in Helpers.GetBlockEnumerator(block, coordinates))
        {
            blockedCoords.Add(tileCoords);
        }
        
        placedBlocks.Add(block);
        
        MissionManager.Instance.HandlePlacedBlockChanged();
    }

    
    public void Remove(GameObject block)
    {
        foreach (Transform tileTransform in block.transform)
        {
            if (tileTransform != block.transform)
            {
                var coords = Helpers.RoundPosition(tileTransform.position); //TODO: fix relative positon
                blockedCoords.Remove(coords);
            }
        }

        placedBlocks.Remove(block);
        
        MissionManager.Instance.HandlePlacedBlockChanged();
    }
    
    public bool CanBePlaced(GameObject block, Vector2Int coordinates)
    {
        foreach (Vector2Int tileCoords in Helpers.GetBlockEnumerator(block, coordinates))
        {
            if (!levelMap.ContainsKey(tileCoords) || blockedCoords.Contains(tileCoords))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsPlaced(GameObject block)
    {
        return placedBlocks.Contains(block);
    }
    
    private void LoadSelectedMap()
    {
        var selectedMap = BriefingManager.Instance.GetSelectedMap();
        Instantiate(selectedMap.levelPrefab);
    }
}
