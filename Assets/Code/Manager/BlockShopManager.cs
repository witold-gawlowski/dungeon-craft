using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockShopManager : MonoBehaviour
{
    public static BlockShopManager Instance { get; private set; }
    
    public Action dataChanged ;

    private List<BlockShopItem> _shopOffer;

    private bool _visitedCurrentDay;

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

    public void Subscribe(BlockShopUIManager ui)
    {
        ui.screenOpened += HandleScreenOpened;
    }

    public void Unsubscribe(BlockShopUIManager ui)
    {
        ui.screenOpened -= HandleScreenOpened;
    }
    
    public List<BlockShopItem> GetShopOffer()
    {
        return _shopOffer;
    }
    
    public void TryBuy(BlockShopItem item)
    {
        var cashManager = CashManager.Instance;
        if (cashManager.GetCash() >= item.Price)
        {
            cashManager.RemoveCash(item.Price);
            BlockInventoryManager.Instance.Add(item.Block, item.Count);
            var result = _shopOffer.Remove(item);
            dataChanged?.Invoke();
        }
    }

    public bool GetVisitedCurrentDay()
    {
        return _visitedCurrentDay;
    }

    private void HandleScreenOpened()
    {
        _visitedCurrentDay = true;
    }

    public void HandleNewDay()
    {
        _visitedCurrentDay = false;
    }

    public void HandleNewStage()
    {
        LoadShopOffer();
    }
   
    private void LoadShopOffer()
    {
        var run = GlobalMapManager.Instance.GetSelectedRunConfig();
        var stage = StageManager.Instance.GetCurrentStage();
        var shopItems = CreateShopOffer(123);

        _shopOffer = shopItems;
    }

    private List<BlockShopItem> CreateShopOffer(int seed)
    {
        Random.InitState(seed);
        int count = Random.Range(8, 15);
        var result = new List<BlockShopItem>();
        for (int i = 0; i < count; i++)
        {
            var newItem = new BlockShopItem
            {
                Price = Random.Range(0, 5) * 5 + 20,
                Count = Random.Range(0, 5) + 4,
                Block = BlockInventoryManager.Instance.GetRandomBlockConfig()
            };
            result.Add(newItem);
        }

        return result;
    }
}
