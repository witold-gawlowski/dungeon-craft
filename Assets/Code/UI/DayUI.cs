using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayUI : MonoBehaviour
{
    [SerializeField] public TMP_Text text;

    public void Start()
    {
        var dayNumber = DayCycleManager.Instance.GetDay();
        SetDay(dayNumber);
    }

    public void SetDay(int number)
    {
        text.text = "Day " + (number + 1);
    }
}
