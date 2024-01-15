using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using ShopItem = LevelShopManager.ShopItem;

public class LevelInventory : MonoBehaviour
{
    public class LevelInventoryItem
    {
        public MapConfig MapConfig;
        public bool IsNew = false;
    }
    public static LevelInventory Instance;
    
    private List<LevelInventoryItem> _levels;

    public Action dataChanged;
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

        _levels = new List<LevelInventoryItem>();
    }

    public void HandleNewDay()
    {
        ClearNewStatus();
    }

    public void Subscribe(BriefingUIManager ui)
    {
        ui.levelButtonPressed += HandleBriefingLevelButtonPressed;
    }

    public void Unsubscribe(BriefingUIManager ui)
    {
        ui.levelButtonPressed -= HandleBriefingLevelButtonPressed;
    }
    
    public List<LevelInventoryItem> GetLevels()
    {
        return _levels;
    }

    public bool HasLevels()
    {
        return _levels.Count > 0;
    }

    public void Add(MapConfig map)
    {
        var newItem = new LevelInventoryItem { };
        newItem.IsNew = true;
        newItem.MapConfig = map;
        _levels.Add(newItem);
        dataChanged?.Invoke();
    }
    public void Remove(MapConfig config)
    {
        foreach (var level in _levels)
        {
            if (level.MapConfig == config)
            {
                _levels.Remove(level);
                break;
            }
        }
        
        dataChanged?.Invoke();
    }

    private void HandleBriefingLevelButtonPressed(MapConfig map)
    {
        ClearNewStatus(map);
    }
    
    private void ClearNewStatus()
    {
        foreach (var level in _levels)
        {
            level.IsNew = false;
        }
        dataChanged?.Invoke();
    }

    private void ClearNewStatus(MapConfig map)
    {
        foreach (var levelWithStatus in _levels)
        {
            if (levelWithStatus.MapConfig == map)
            {
                levelWithStatus.IsNew = false;
                break;
            }
        }
        dataChanged?.Invoke();
    }
}
