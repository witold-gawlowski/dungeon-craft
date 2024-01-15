using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelShopManager : MonoBehaviour
{
    public class ShopItem
    {
        public MapConfig Map;
        public bool Bought = false;
        public int Price;
    }
    
    [SerializeField] private List<MapConfig> configs;
    
    private List<ShopItem> _shopItems;
    private List<MapConfig> _completerMaps;

    public event Action dataChangedEvent;

    public static LevelShopManager Instance { get; private set; }

    private bool _fadePlayedCurrentApproach;
    private bool _dayAnnouncementShown;

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
    
    public void Subscribe(LevelShopUIManager ui)
    {
        ui.levelBouttonPressed += HandleTryBuy;
        ui.fadePlayedEvent += HandleFadePlayedEvent;
        ui.dayAnnouncmentShowEvent += HandleDayAnnoucmentShown;
    }

    public void Unsubscribe(LevelShopUIManager ui)
    {
        ui.levelBouttonPressed -= HandleTryBuy;
        ui.fadePlayedEvent -= HandleFadePlayedEvent;
        ui.dayAnnouncmentShowEvent -= HandleDayAnnoucmentShown;
    }

    public void SetFadePlayed(bool value = true)
    {
        _fadePlayedCurrentApproach = value;
    }

    public void SetDayAnnoucedShown(bool value = true)
    {
        _dayAnnouncementShown = value;
    }

    public bool IsMapACompleter(MapConfig map)
    {
        return _completerMaps.Contains(map);
    }

    public void ClearBought()
    {
        List<ShopItem> cleanList = new List<ShopItem>();
        foreach (var item in _shopItems)
        {
            if (item.Bought == false)
            {
                cleanList.Add(item);
            }
        }
        _shopItems = cleanList;

        dataChangedEvent?.Invoke();
    }
    
    public bool GetFadePlayed()
    {
        return _fadePlayedCurrentApproach;
    }

    public bool GetDayAnnouncementShown()
    {
        return _dayAnnouncementShown;
    }
        
    public List<ShopItem> GetShopItems()
    {
        return _shopItems;
    }

    public List<MapConfig> GetCompleterMaps()
    {
        return _completerMaps;
    }

    private void HandleFadePlayedEvent()
    {
        SetFadePlayed(true);
    }

    private void HandleDayAnnoucmentShown()
    {
        SetDayAnnoucedShown(true);
    }
    
    private void HandleTryBuy(ShopItem item)
    {
        var cashMAnager = CashManager.Instance;
        if (cashMAnager.GetCash() >= item.Price)
        {
            cashMAnager.RemoveCash(item.Price);
            BuySelected(item);
            LevelInventory.Instance.Add(item.Map);
        }
    }
    
    private void BuySelected(ShopItem config)
    {
        var index = _shopItems.FindIndex(item => config == item);
        var newItem = _shopItems[index];
        newItem.Bought = true;
        _shopItems[index] = newItem;
        dataChangedEvent?.Invoke();
    }

    public void HandleNewDay()
    {
        _fadePlayedCurrentApproach = false;
        _dayAnnouncementShown = false;
    }

    public void HandleNewStage()
    {
        LoadRunConfig();
    }
    
    void LoadRunConfig()
    {
        var runConfig = GlobalMapManager.Instance.GetSelectedRunConfig();

        var stage = StageManager.Instance.GetCurrentStage();
        var stageMaps = runConfig.GetMaps(stage);
        
        _shopItems = new List<ShopItem>();

        foreach (var map in stageMaps)
        {
            var newShopItem = new ShopItem { Map = map, Price = 123};
            _shopItems.Add(newShopItem);
        }

        _completerMaps = runConfig.GetStageCompleters(stage);
    }
}
