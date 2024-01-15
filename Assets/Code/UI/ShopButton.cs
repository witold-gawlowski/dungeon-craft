using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private Image stars1;
    [SerializeField] private Image stars2;

    [SerializeField] private float decaySpeed = 0.1f;
    [SerializeField] private AnimationCurve borderPulse;
    [SerializeField] private AnimationCurve glimmerPulse;

    protected bool shopMode = false;
    protected bool highlight;
    [SerializeField] protected bool glimmer;

    public void ButtonPressed()
    {
        if (shopMode)
        {
            if (buttonImage.fillAmount > 0.55)
            {
                OnActionTriggered();
            }

            buttonImage.fillAmount += 0.45f;
        }
        else
        {
            OnActionTriggered();
        }

        highlight = false;
        glimmer = false;
    }

    void Update()
    {
        if (buttonImage.fillAmount > 0)
        {
            buttonImage.fillAmount -= Time.deltaTime * decaySpeed;
        }
        
        if (stars1.gameObject.activeSelf != glimmer)
        {
            stars1.gameObject.SetActive(glimmer);
        }
        if (stars2.gameObject.activeSelf != glimmer)
        {
            stars2.gameObject.SetActive(glimmer);
        }
        
        Color c = borderImage.color;
        c = stars1.color;
        if (glimmer)
        {
            c.a = glimmerPulse.Evaluate(Time.time);
            stars1.color = c;

            c.a = glimmerPulse.Evaluate(Time.time + 1.5f);
            stars2.color = c;
        }
    }

    public void SetHighlight(bool value)
    {
        highlight = value;
        borderImage.gameObject.SetActive(value);
    }

    public void SetGlimmer(bool value)
    {
        glimmer = value;
        stars1.gameObject.SetActive(value);
        stars2.gameObject.SetActive(value);
    }
    
    protected virtual void OnActionTriggered()
    {
        
    }
}
