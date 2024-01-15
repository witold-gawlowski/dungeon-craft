using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class RunButtonScript : MonoBehaviour
{
    [SerializeField] private int seed;
    [SerializeField] private RegionConfig regionConfig;
    
    [SerializeField] private GameObject questionmarkObject;
    [SerializeField] private GameObject borderObject;
    [SerializeField] private Image blockImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject conqueredImageObject;
    [SerializeField] private Button button;

    public Action runButtonPressedEvent;
    
    public void OnPress()
    {
        runButtonPressedEvent?.Invoke();
    }
    
    public bool ShouldBeHidden()
    {
        return seed == 0 || regionConfig == null;
    }

    public void RandomSeed()
    {
        seed = UnityEngine.Random.Range(1, 1000);
    }

    // TODO: use on start of all set-visuals functions
    public void SetHiddenVisuals()
    {
        SetAllVisualsDisabled();
    }

    public void ShowEditorVisuals()
    {
        InitCommonVisuals();
    }

    public void ShowConqueredVisuals()
    {
        InitCommonVisuals();

        conqueredImageObject.SetActive(true);
    }

    public void ShowSelectedNotDiscoveredVisuals()
    {
        InitCommonVisuals();

        questionmarkObject.SetActive(true);
        borderObject.SetActive(true);
        button.enabled = true;
    }

    public void ShowSelectedDiscoveredVisuals(BlockConfig reward)
    {
        InitCommonVisuals();

        borderObject.SetActive(true);
        blockImage.gameObject.SetActive(true);
        blockImage.sprite = reward.icon;
        button.enabled = true;
    }

    public void ShowNotSelectedDisoveredVisuals(BlockConfig reward)
    {
        InitCommonVisuals();

        blockImage.gameObject.SetActive(true);
        blockImage.sprite = reward.icon;
        button.enabled = true;
    }

    public void ShowNotSelectedNotDiscoveredVisuals()
    {
        InitCommonVisuals();

        button.enabled = true;
    }

    public int GetRunSeed()
    {
        return seed;
    }

    public RegionConfig GetRegionConfig()
    {
        return regionConfig;
    }

    private void InitCommonVisuals()
    {
        SetAllVisualsDisabled();
        backgroundImage.enabled = true;

        Color newCOlor = regionConfig ? regionConfig.GetRegionColor() : Color.black;
        newCOlor.a = regionConfig ? 0.4f : 0.1f;
        backgroundImage.color = newCOlor;
    }

    private void SetAllVisualsDisabled()
    {
        questionmarkObject.SetActive(false);
        borderObject.SetActive(false);
        blockImage.gameObject.SetActive(false);
        backgroundImage.enabled = false;
        button.enabled = false;
    }
}
