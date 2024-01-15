using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SectionButtonBase : MonoBehaviour
{
    [SerializeField] protected string sceneName;
    
    [Space(10)]
    [SerializeField] protected AnimationCurve highlightPulse;

    protected Color _initialColor;
    protected Button _button;
    protected Image _image;


    protected bool _isHighlighted;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _isHighlighted = false;
        _initialColor = _image.color;
    }

    private void Start()
    {
        Subscribe();
        UpdateButtonVisuals();
    }

    private void Update()
    {
        if (_isHighlighted)
        {
            Color c = _image.color;
            c.a = highlightPulse.Evaluate(Time.time);
            _image.color = c;
        }
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    protected virtual void Subscribe()
    {

    }

    protected virtual void Unsubscribe()
    {

    }

    protected void HandleDataChanged()
    {
        UpdateButtonVisuals();
    }

    public string GetSceneName()
    {
        return sceneName;
    }
    
    public void OnPress()
    {   
        SceneManager.LoadScene(sceneName);
    }
    
    private void UpdateButtonVisuals()
    {
        if (ShouldBeVisible())
        {
            SetVisible(true);
            if (ShouldBeSelected())
            {
                SetButtonSelectedVisual();
            }
            else if (ShouldBeHighlighted())
            {
                SetHighlightedVisual();
            }
            else
            {
                SetNotSelectedNotHighlightedVisual();
            }
        }
        else
        {
            SetVisible(false);
        }
    }
    
    protected virtual bool ShouldBeVisible()
    {
        return true;
    }

    private bool ShouldBeSelected()
    {
        return GetSceneName() == SceneManager.GetActiveScene().name;
    }

    protected virtual bool ShouldBeHighlighted()
    {
        return false;
    }

    private void SetVisible(bool value)
    {
        _button.enabled = value;
        _image.enabled = value;
    }
    private void SetNotSelectedNotHighlightedVisual()
    {
        _image.color = _initialColor;
    }
    private void SetHighlightedVisual()
    {
        _isHighlighted = true;
    }
    public void SetButtonSelectedVisual()
    {
        var newColor = _image.color;
        newColor.a = 1;
        _image.color = newColor;
    }
}
