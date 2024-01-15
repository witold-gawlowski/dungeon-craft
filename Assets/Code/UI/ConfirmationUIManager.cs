using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmationUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text caption;
    
    public Action screenOpened;

    private void Start()
    {
        ConfirmationManager.Instance.Subscribe(this);
        
        screenOpened?.Invoke();
    }

    private void OnDestroy()
    {
        ConfirmationManager.Instance.Unsubscribe(this);
        ConfirmationManager.Instance.HandleScreenLeave();
    }

    public void UpdateUI(float timer)
    {
        caption.text = "Starting in " + Mathf.RoundToInt(timer);
    }
}
