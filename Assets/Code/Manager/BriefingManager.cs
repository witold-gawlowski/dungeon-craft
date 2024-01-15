using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class BriefingManager : MonoBehaviour
{
    public static BriefingManager Instance;

    private int _target;

    private MapConfig _selectedMap;
    
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
    }

    public void Subscribe(BriefingUIManager ui)
    {
        ui.targetUpdatedEvent += HandleTargetUpdatedEvent;
        ui.levelButtonPressed += HandleSelectedMapUpdatedEvent;
    }

    public void Unsubscribe(BriefingUIManager ui)
    {
        ui.targetUpdatedEvent -= HandleTargetUpdatedEvent;
        ui.levelButtonPressed -= HandleSelectedMapUpdatedEvent;
    }
    
    private void HandleTargetUpdatedEvent(float value)
    {
        var levelSize = Helpers.GetLevelSize(_selectedMap.levelPrefab);
        SetTarget(Mathf.RoundToInt(value * levelSize));
    }

    private void HandleSelectedMapUpdatedEvent(MapConfig levelMap)
    {
        SelectLevel(levelMap);
    }

    public MapConfig GetSelectedMap()
    {
        return _selectedMap;
    }

    public bool HasLevelSelected()
    {
        return _selectedMap != null;
    }

    public void ClearSelectedLevel()
    {
        _selectedMap = null;
        dataChanged?.Invoke();
    }
    
    public void SetTarget(int value)
    {
        _target = value;
        dataChanged?.Invoke();
    }

    public int GetTarget()
    {
        return _target;
    }

    public int GetReward()
    {
        return _target * 10;
    }
    
    private void SelectLevel(MapConfig levelMap)
    {
        _selectedMap = levelMap;
        SetTarget(0);
        dataChanged?.Invoke();
    }
}
