using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public Action<int, int> scoreChangedEvent;
    public Action<float> timeChangedEvent;

    private MissionUIManager ui;
    
    private float timeRemaining;
    private bool clockRunning;
    
    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        UpdateScore();

        timeRemaining = 60;

        clockRunning = true;
    }

    private void FixedUpdate()
    {
        if (clockRunning)
        {
            timeRemaining -= Time.deltaTime;

            timeChangedEvent?.Invoke(timeRemaining);

            if (timeRemaining < 0)
            {
                //TODO
            }
        }
    }

    public void Subscribe(MissionUIManager ui)
    {
        this.ui = ui;
        
        ui.fadeEndedEvent += HandleFadeEndedEvent;
        ui.giveUpEvent += HandleGiveUpEvent;
    }

    public void Unsubscribe(MissionUIManager ui)
    {
        this.ui = null;
        
        //TODO make the functions in unsubscrue and subscribe match
        ui.fadeEndedEvent -= HandleFadeEndedEvent;
        ui.giveUpEvent -= HandleGiveUpEvent;
    }

    public void HandlePlacedBlockChanged()
    {
        UpdateScore();
        
        if (IsMissionCompleted())
        {
            ui.HandleMissionCompleted(IsRunCompleted());
        }
    }

    private void HandleGiveUpEvent()
    {
        ui.HandleMissionFailed();
    }

    private void HandleFadeEndedEvent()
    {
        if(IsMissionCompleted())
        {
            bool isRunCompleted = IsRunCompleted();
            HandleMissionCompleted();
            if (isRunCompleted)
            {
                SceneManager.LoadScene("8-GlobalMap");
            }
            else
            {
                SceneManager.LoadScene("2-LevelShop");
            }
        }
        else
        {
            HandleMissionFailed();
            SceneManager.LoadScene("2-LevelShop");
        }
    }

    private bool IsMissionCompleted()
    {
        var score = MapManager.GetInstance().GetCoveredArea();
        var target = BriefingManager.Instance.GetTarget();

        return score >= target;
    }

    private bool IsRunCompleted()
    {
        var currentMap = BriefingManager.Instance.GetSelectedMap();
        return LevelShopManager.Instance.IsMapACompleter(currentMap) && StageManager.Instance.IsFinalStage();
    }

    private void StopClock()
    {
        clockRunning = false;
    }
    private void UpdateScore()
    {
        var score = MapManager.GetInstance().GetCoveredArea();
        var target = BriefingManager.Instance.GetTarget();
        scoreChangedEvent?.Invoke(score, target);
    }

    //TODO: move those calls to dayCycleManager
    private void HandleMissionCompleted()
    {
        //TODO: block all actions
        StopClock();

        var reward = BriefingManager.Instance.GetReward();
        CashManager.Instance.AddCash(reward);
        
        StageManager.Instance.HandleLevelCompleted();
        
        CleanUpAfterMissionCompleted();
        
        DayCycleManager.Instance.MakeNewDay();

    }

    private void CleanUpAfterMissionCompleted()
    {
        CleanUpAfterMissionCommon();

        LevelShopManager.Instance.SetDayAnnoucedShown(false);
    }
    private void HandleMissionFailed()
    {
        CleanUpAfterMissionCommon();

        LevelShopManager.Instance.ClearBought();
    }

    private void CleanUpAfterMissionCommon()
    {
        BlockSelectionManager.Instance.Clear();

        var selectedMap = BriefingManager.Instance.GetSelectedMap();
        LevelInventory.Instance.Remove(selectedMap);

        BriefingManager.Instance.ClearSelectedLevel();
    }
}
