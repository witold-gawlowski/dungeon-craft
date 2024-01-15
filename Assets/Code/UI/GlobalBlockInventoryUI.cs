using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBlockInventoryUI : MonoBehaviour
{
    [SerializeField] private Transform blockImageParent;
    [SerializeField] private GameObject blockImagePrefab;

    private void Start()
    {
        Clear();
        CreateInventoryButtons();    
    }
    
    private void CreateInventoryButtons()
    {
        var inventoryData = GlobalBlockInventory.Instance.GetBlocks();
        
        foreach (var p in inventoryData)
        {
            if (p.Value > 0)
            {
                var newButton = Instantiate(blockImagePrefab, blockImageParent);
                var buttonScript = newButton.GetComponent<BlockButtonScript>();
                buttonScript.InitSelectionButton(p.Key.icon, p.Value);
            }
        }
    }
    
    private void Clear()
    {
        foreach (Transform t in blockImageParent)
        {
            if (t != blockImageParent)
            {
                Destroy(t.gameObject);
            }
        }
    }
}
