using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockButtonScript : ShopButton
{
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Image blockIcon;

    public Action buy;
    public Action toggleSelection;
    
    public void InitShopButton(Sprite iconSprite, int count, int price)
    {
        shopMode = true;
        blockIcon.sprite = iconSprite;
        priceText.gameObject.SetActive(true);
        priceText.text = price + "$";
        countText.text = "x" + count.ToString();
    }

    public void InitSelectionButton(Sprite iconSprite, int count)
    {
        shopMode = false;
        blockIcon.sprite = iconSprite;
        countText.text = "x" + count.ToString();
        priceText.gameObject.SetActive(false);
    }
    
    protected override void OnActionTriggered()
    {
        CallEvents();
    }

    public void CallEvents()
    {
        buy?.Invoke();
        toggleSelection?.Invoke();
    }
    
}
