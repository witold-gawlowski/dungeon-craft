using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class BlockInventoryManager : MonoBehaviour
{
    public static BlockInventoryManager Instance { get; private set; }

    public Action dataChanged;
    
    private List<BlockConfig> _blockConfigs;
    private Dictionary<BlockConfig, int> _inventory;
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
        
        _blockConfigs = new List<BlockConfig>(Resources.LoadAll<BlockConfig>("BlockConfigs"));

        //TODO: move to start?
        dataChanged?.Invoke();
    }

    private void Start()
    {
        var initialBlocks = GlobalBlockInventory.Instance.GetBlocks();
        _inventory = new Dictionary<BlockConfig, int>(initialBlocks);
    }

    public void Subscribe(BlockSelectionUiManager ui)
    {
        ui.blockSelectionCountChangeRequestedEvent += HandleSelectionChange;
    }

    public void Unsubscribe(BlockSelectionUiManager ui)
    {
        ui.blockSelectionCountChangeRequestedEvent -= HandleSelectionChange;
    }
    
    public void Add(BlockConfig block, int count)
    {
        if (_inventory.ContainsKey(block))
        {
            _inventory[block] = _inventory[block] + count;
        }
        else
        {
            _inventory.Add(block, count);
        }
        
        dataChanged?.Invoke();
    }

    public void Remove(BlockConfig block, int count)
    {
        if (_inventory.ContainsKey(block))
        {
            _inventory[block] = Mathf.Max(0, _inventory[block] - count);
        }
        
        dataChanged?.Invoke();
    }

    public int GetCount(BlockConfig block)
    {
        if (_inventory.ContainsKey(block))
        {
            return _inventory[block];
        }
        return 0;
    }

    public Dictionary<BlockConfig, int> GetBlocks()
    {
        return _inventory;
    }

    public bool HasAnyBlocks()
    {
        foreach (var count in _inventory)
        {
            if (count.Value > 0)
            {
                return true;
            }
        }
        return false;
    }
    
    public BlockConfig GetRandomBlockConfig()
    {
        Assert.IsTrue(_blockConfigs is { Count: > 0 });
        int index = Random.Range(0, _blockConfigs.Count);
        return _blockConfigs[index];
    }

    private void HandleSelectionChange(BlockConfig block, int count)
    {
        if (count > 0)
        {
            Remove(block, count);
        }

        if (count < 0)
        {
            Add(block, -count);
        }
    }
}
