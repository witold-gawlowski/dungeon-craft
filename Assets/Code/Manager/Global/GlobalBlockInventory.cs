using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalBlockInventory : MonoBehaviour
{
    [Serializable]
    public class InitialBlockCountConfig
    {
        public BlockConfig block;
        public int count;
    }
    public static GlobalBlockInventory Instance { get; private set; }

    [SerializeField] private List<InitialBlockCountConfig> initialBlocks;
    
    private Dictionary<BlockConfig, int> _blocks;
    
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

        _blocks = new Dictionary<BlockConfig, int>();
    }

    void Start()
    {
        foreach (var count in initialBlocks)
        {
            _blocks.Add(count.block, count.count);
        }
    }

    public void AddBlock(BlockConfig block, int count)
    {
        if (_blocks.ContainsKey(block))
        {
            _blocks[block] += count;
        }
        else
        {
            _blocks.Add(block, count);
        }
    }

    public int GetBlockCount(BlockConfig block)
    {
        if (_blocks.TryGetValue(block, out var count))
        {
            return count;
        }

        return 0;
    }

    public bool Contains(BlockConfig block)
    {
        return GetBlockCount(block) > 0;
    }

    public Dictionary<BlockConfig, int> GetBlocks()
    {
        return _blocks;
    }
    
}
