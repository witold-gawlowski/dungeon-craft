using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalConfirmationManager : MonoBehaviour
{
    [SerializeField] private TMP_Text confirmationText;
    
    private bool _doCountdown;
    private float _timer;
    private bool _moveToTargetScreenTriggered;

    private void Start()
    {
        StartCountdown();
    }

    private void Update()
    {
        if (_doCountdown)
        {
            _timer -= Time.deltaTime;
            if (_timer < -0.5f && ! _moveToTargetScreenTriggered)
            {
                GlobalFlowManager.Instance.HandleRunStart();
                HandleRunStart();
            }
            else
            {
                UpdateUI(_timer);
            }
        }
    }

    private void StartCountdown()
    {
        _doCountdown = true;
        _timer = 3.5f;
    }
    
    private void HandleRunStart()
    {
        _moveToTargetScreenTriggered = true;
        _doCountdown = false;
    }
    
    public void UpdateUI(float timer)
    {
        confirmationText.text = "Starting in " + Mathf.RoundToInt(timer);
    }
}
