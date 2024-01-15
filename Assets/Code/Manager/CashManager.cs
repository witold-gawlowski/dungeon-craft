using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    [SerializeField] private int startCash;
    private int cash; 
    public static CashManager Instance { get; private set; }

    public Action<int> cashIsUpdatedEvent;

    private void Awake()
    {
        if (Instance && this != Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        cash = startCash;
    }

    public int GetCash()
    {
        return cash;
    }
    
    public void AddCash(int value)
    {
        cash += value;
        cashIsUpdatedEvent?.Invoke(cash);
    }

    public void RemoveCash(int value)
    {
        cash -= value;
        cashIsUpdatedEvent?.Invoke(cash);
    }
}
