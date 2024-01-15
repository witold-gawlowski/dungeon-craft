using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    
    private int _stage;
    private LevelShopUIManager _ui;
    private Color _currentStageColor;
    
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

    private void Start()
    {
        _stage = 0;
        HandleNewStage();
    }

    public void Subscribe(LevelShopUIManager ui)
    {
        _ui = ui;
    }

    public void Unsubscribe(LevelShopUIManager ui)
    {
        _ui = null;
    }
    
    public void HandleLevelCompleted()
    {
        var completedMap = BriefingManager.Instance.GetSelectedMap();

        var selectedRunConfig = GlobalMapManager.Instance.GetSelectedRunConfig();
        var maxStage = selectedRunConfig.GetStageCount() - 1;
        var completerMaps = LevelShopManager.Instance.GetCompleterMaps();

        if (completerMaps.Contains(completedMap))
        {
            if (_stage < maxStage)
            {
                _stage++;
                HandleNewStage();
            }
            else
            {
                GlobalMapManager.Instance.HandleRunCompleted();
            }
        }
    }

    public bool IsFinalStage()
    {
        var selectedRunConfig = GlobalMapManager.Instance.GetSelectedRunConfig();
        var maxStage = selectedRunConfig.GetStageCount() - 1;

        return _stage == maxStage;
    }

    public Color GetStageColor()
    {
        return _currentStageColor;
    }

    public int GetCurrentStage()
    {
        return _stage;
    }

    private void HandleNewStage()
    {
        LevelShopManager.Instance.HandleNewStage();
        BlockShopManager.Instance.HandleNewStage();

        _currentStageColor = GetRandomColor();
    }

    private Color GetRandomColor()
    {
        var hue = Random.value;
        return Color.HSVToRGB(hue, 0.62f, 0.47f);
    }
    
}
