using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CashUI : MonoBehaviour
{
    [SerializeField] private TMP_Text cashText;
    
    private void Awake()
    {
        CashManager.Instance.cashIsUpdatedEvent += SetCash;
        
        var value = CashManager.Instance.GetCash();
        SetCash(value);
    }

    private void OnDestroy()
    {
        CashManager.Instance.cashIsUpdatedEvent -= SetCash;
    }

    public void SetCash(int value)
    {
        cashText.text = value + "$";
    }
}
