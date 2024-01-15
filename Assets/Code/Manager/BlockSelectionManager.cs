using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelectionManager : MonoBehaviour
{
    public static BlockSelectionManager Instance { get; private set; }

    public Action dataChanged;
    
    private Dictionary<BlockConfig, int> _selected;
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

        _selected = new Dictionary<BlockConfig, int>();
        
        dataChanged?.Invoke();
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
        if (_selected.ContainsKey(block))
        {
            _selected[block] = _selected[block] + count;
        }
        else
        {
            _selected.Add(block, count);
        }

        dataChanged?.Invoke();
    }

    public void Remove(BlockConfig block, int count)
    {
        if (_selected.ContainsKey(block))
        {
            _selected[block] = Mathf.Max(0, _selected[block] - count);
        }
        
        dataChanged?.Invoke();
    }

    public void Clear()
    {
        _selected = new Dictionary<BlockConfig, int>();
        dataChanged?.Invoke();
    }

    public int GetCount(BlockConfig block)
    {
        if (_selected.ContainsKey(block))
        {
            return _selected[block];
        }
        return 0;
    }

    public bool HasBlocks()
    {
        foreach (var count in _selected)
        {
            if (count.Value > 0)
            {
                return true;
            }
        }
        return false;
    }
    
    public Dictionary<BlockConfig, int> GetBlocks()
    {
        return _selected;
    }

    private void HandleSelectionChange(BlockConfig block, int count)
    {
        if (count > 0)
        {
            Add(block, count);
        }

        if (count < 0)
        {
            Remove(block, -count);
        }
    }
}
