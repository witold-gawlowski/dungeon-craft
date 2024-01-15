using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ConfirmationManager : MonoBehaviour
{
    public static ConfirmationManager Instance { get; private set; }
    
    private float _timer;
    private bool _visitedCurrentDay;
    private ConfirmationUIManager _ui;
    private bool _doCountdown;
    private bool _moveToMissionTriggered;
    
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

        _doCountdown = false;
    }
    
    private void Update()
    {
        if (_doCountdown)
        {
            _timer -= Time.deltaTime;
            if (_timer < -0.5f && ! _moveToMissionTriggered)
            {
                SceneManager.LoadScene("7-Mission");
                HandleMissionStart();
            }
            _ui.UpdateUI(_timer);
        }
    }

    public void Subscribe(ConfirmationUIManager ui)
    {
        _ui = ui;
        ui.screenOpened += HandleScreenOpened;
    }
    
    public void Unsubscribe(ConfirmationUIManager ui)
    {
        _ui = null;
        ui.screenOpened -= HandleScreenOpened;
    }
    
    public bool GetVisitedCurrentDay()
    {
        return _visitedCurrentDay;
    }

    private void HandleScreenOpened()
    {
        _visitedCurrentDay = true;
        _timer = 3.5f;
        _doCountdown = true;
        _moveToMissionTriggered = false;
    }
    
    public void HandleScreenLeave()
    {
        _doCountdown = false;
    }

    public void HandleNewDay()
    {
        _visitedCurrentDay = false;
    }

    private void HandleMissionStart()
    {
        LevelShopManager.Instance.SetFadePlayed(false);
        _moveToMissionTriggered = true;
        _doCountdown = false;
    }


}
