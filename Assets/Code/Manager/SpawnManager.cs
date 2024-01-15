using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnManager : MonoBehaviour
{

    private Dictionary<GameObject, BlockConfig> _configs;
    private Dictionary<BlockConfig, int> _snappedCount;
    public static SpawnManager Instance { get; private set; }

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

        _configs = new Dictionary<GameObject, BlockConfig>();
        _snappedCount = new Dictionary<BlockConfig, int>();
    }
    
    private void Start()
    {
        DragManagerScript.Instance.freeBlockDragStart += HandleFreeBlockDragStart;
        SpawnInitialBlocks();
    }
    
    private void OnDestroy()
    {
        DragManagerScript.Instance.freeBlockDragStart -= HandleFreeBlockDragStart;
    }

    public void HandleBlockSnapped(GameObject block)
    {
        var config = _configs[block];
        AddSnapped(config);
    }

    public void HandleBLockUnsnapped(GameObject block)
    {
        var config = _configs[block];
        RemoveSnapped(config);
    }

    public void HandleBlockDragEndedWhenNotSnapped(GameObject block)
    {
        var blockConfig = _configs[block];
        Assert.IsNotNull(blockConfig);
        var count = BlockSelectionManager.Instance.GetCount(blockConfig);
        if (count > GetSnappedCount(blockConfig) + 1)
        {
            Destroy(block);
        }
    }


    private void AddSnapped(BlockConfig block)
    {
        if (_snappedCount.ContainsKey(block))
        {
            _snappedCount[block]++;
        }
        else
        {
            _snappedCount.Add(block, 1);
        }
    }

    private void RemoveSnapped(BlockConfig block)
    {
        _snappedCount[block]--;
    }

    private void HandleFreeBlockDragStart(GameObject block)
    {
        var blockConfig = _configs[block];
        Assert.IsNotNull(blockConfig);
        var count = BlockSelectionManager.Instance.GetCount(blockConfig);
        if (count > GetSnappedCount(blockConfig) + 1)
        {
            SpawnNewBlock(blockConfig, block.transform.position, block.transform.rotation.eulerAngles.z);
        }
    }

    private int GetSnappedCount(BlockConfig block)
    {
        if (!_snappedCount.ContainsKey(block))
        {
            return 0;
        }

        return _snappedCount[block];
    }

    private void SpawnInitialBlocks()
    {
        var blockCounts = BlockSelectionManager.Instance.GetBlocks();
        
        int index = 0;
        foreach (var count in blockCounts)
        {
            if (count.Value > 0)
            {
                var screenPos = Helpers.GetUIBlockPosition(index);
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                var block = SpawnNewBlock(count.Key, worldPos);
                index++;
            }
        }
    }
    
    private GameObject SpawnNewBlock(BlockConfig config, Vector3 position, float rotation = 0)
    {
        var rotationQuat = Quaternion.AngleAxis(rotation, Vector3.forward);
        var newBlock = Instantiate(config.prefab, position, rotationQuat);
        _configs.Add(newBlock, config);
        return newBlock;
    }
}
