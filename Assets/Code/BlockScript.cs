using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> tiles;
    
    [SerializeField] private Color freeColor;
    [SerializeField] private Color snappedColor;
    private void Awake()
    {
        tiles = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
    }

    public void SetToFreeColor()
    {
        foreach (var t in tiles)
        {
            t.color = freeColor;
        }
    }

    public void SetToSnappedColor()
    {
        foreach (var t in tiles)
        {
            t.color = snappedColor;
        }
    }
    
}
