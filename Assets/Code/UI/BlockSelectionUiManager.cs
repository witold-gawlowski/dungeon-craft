using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockSelectionUiManager : MonoBehaviour
{
    [SerializeField] private GameObject blockButtonPrefab;
    
    [SerializeField] private Transform selectedParent;
    [SerializeField] private Transform inventoryParent;

    [SerializeField] private GameObject emptySelectedText;
    [SerializeField] private GameObject emptyInventoryText;

    public Action<BlockConfig, int> blockSelectionCountChangeRequestedEvent;
    
    private void Awake()
    {
        BlockSelectionManager.Instance.dataChanged += OnSelectedDataChanged;
        BlockInventoryManager.Instance.dataChanged += OnInventoryDataChanged;

        OnSelectedDataChanged();
        OnInventoryDataChanged();
        
        BlockSelectionManager.Instance.Subscribe(this);
        BlockInventoryManager.Instance.Subscribe(this);
    }

    private void OnDestroy()
    {
        BlockSelectionManager.Instance.dataChanged -= OnSelectedDataChanged;
        BlockInventoryManager.Instance.dataChanged -= OnInventoryDataChanged;
        
        BlockSelectionManager.Instance.Unsubscribe(this);
        BlockInventoryManager.Instance.Unsubscribe(this);
    }

    private void OnSelectedDataChanged()
    {
        Clear(selectedParent);
        CreateSelectedButtons();
    }

    private void OnInventoryDataChanged()
    {
        Clear(inventoryParent);
        CreateInventoryButtons();
    }

    private void CreateSelectedButtons()
    {
        var selectedData = BlockSelectionManager.Instance.GetBlocks();
        bool isSectionEmpty = true;
        foreach (var p in selectedData)
        {
            if (p.Value > 0)
            {
                var newButton = Instantiate(blockButtonPrefab, selectedParent);
                var buttonScript = newButton.GetComponent<BlockButtonScript>();
                buttonScript.InitSelectionButton(p.Key.icon, p.Value);
                buttonScript.toggleSelection += delegate {blockSelectionCountChangeRequestedEvent?.Invoke(p.Key, -1);};
                
                isSectionEmpty = false;
            }
        }
        
        emptySelectedText.SetActive(isSectionEmpty);
    }

    private void CreateInventoryButtons()
    {
        var inventoryData = BlockInventoryManager.Instance.GetBlocks();
        bool isSectionEmpty = true;
        foreach (var p in inventoryData)
        {
            if (p.Value > 0)
            {
                var newButton = Instantiate(blockButtonPrefab, inventoryParent);
                var buttonScript = newButton.GetComponent<BlockButtonScript>();
                buttonScript.InitSelectionButton(p.Key.icon, p.Value);
                buttonScript.toggleSelection += delegate {blockSelectionCountChangeRequestedEvent?.Invoke(p.Key, 1);};
                
                isSectionEmpty = false;
            }
        }
        
        emptyInventoryText.SetActive(isSectionEmpty);
    }
    
    private void Clear(Transform parent)
    {
        foreach (Transform t in parent)
        {
            if (t != parent)
            {
                Destroy(t.gameObject);
            }
        }
    }
}
