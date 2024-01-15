using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockShopUIManager : MonoBehaviour
{
    [SerializeField] private GameObject blockShopButtonPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject emptyInfoText;

    public Action screenOpened;
    
    private void Awake()
    {
        BlockShopManager.Instance.dataChanged += HandleDataChanged;

        BlockShopManager.Instance.Subscribe(this);
    }

    private void Start()
    { 
        HandleDataChanged();
        
        screenOpened?.Invoke();
    }

    private void OnDestroy()
    {
        BlockShopManager.Instance.dataChanged -= HandleDataChanged;
        
        BlockShopManager.Instance.Unsubscribe(this);
    }

    private void HandleDataChanged()
    {
        Clear();
        
        var offer = BlockShopManager.Instance.GetShopOffer();
        foreach (var item in offer)
        {
            var newButton = Instantiate(blockShopButtonPrefab, parent);
            var buttonScript = newButton.GetComponent<BlockButtonScript>();
            buttonScript.InitShopButton(item.Block.icon, item.Count, item.Price);
            buttonScript.buy += () => BlockShopManager.Instance.TryBuy(item);
        }
        
        emptyInfoText.SetActive(offer.Count == 0);
    }

    private void Clear()
    {
        foreach (Transform t in parent.transform)
        {
            if (t != parent.transform)
            {
                Destroy(t.gameObject);
            }
        }
    }
}
