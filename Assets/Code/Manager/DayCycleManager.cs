using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    public static DayCycleManager Instance { get; private set; }
    
    private int dayNumber;
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

        dayNumber = -1;
    }
    private void Start()
    {
        MakeNewDay();
    }

    public int GetDay()
    {
        return dayNumber;
    }
    
    //TODO: move these calls to EventHandler 
    public void MakeNewDay()
    {
        dayNumber++;

        BlockShopManager.Instance.HandleNewDay();
        LevelInventory.Instance.HandleNewDay();
        LevelShopManager.Instance.HandleNewDay();
        ConfirmationManager.Instance.HandleNewDay();
    }
}
