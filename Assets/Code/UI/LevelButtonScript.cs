using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LevelButtonScript : ShopButton
{
    [SerializeField] private Image levelIcon;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject checkObject;
    [SerializeField] private Button button;
    [SerializeField] private GameObject requiredIcon;
    
    public Action buttonPressed;
    public void InitShopButton(Sprite icon, int price, int time, bool bought, bool required = false)
    {
        shopMode = true;
        levelIcon.sprite = icon;
        priceText.gameObject.SetActive(true);
        priceText.text = price + "$";
        requiredIcon.SetActive(required);
            
        if (bought)
        {
            Color newColor = fillImage.color;
            newColor.a = 0.1f;
            fillImage.color = newColor;

            newColor = priceText.color;
            newColor.a = 0.2f;
            priceText.color = newColor;

            newColor = levelIcon.color;
            newColor.a = 0.3f;
            levelIcon.color = newColor;
            
            checkObject.SetActive(true);
            button.interactable = false;
        }
        //UpdateTime(time);
    }
    public void InitSelectionButton(Sprite icon, int time, bool selected = false, bool isNew = false, bool required = false)
    {
        shopMode = false;
        levelIcon.sprite = icon;
        //UpdateTime(time);
        priceText.gameObject.SetActive(false);
        SetHighlight(selected);
        glimmer = isNew;
        requiredIcon.SetActive(required);
    }

    public void UpdateTime(int time)
    {
        if (time >= 60)
        {
            if (time % 60 == 0)
            {
                timeText.text = string.Format("{0}m", time / 60);
            }
            else
            {
                timeText.text = string.Format("{0}m{1}s", time / 60, time % 60);
            }
        }
        else
        {
            timeText.text = string.Format("{0}s", time % 60);
        }
    }

    protected override void OnActionTriggered()
    {
        InvokeButtonPressed();
    }

    public void InvokeButtonPressed()
    {
        buttonPressed?.Invoke();
    }
    
}
