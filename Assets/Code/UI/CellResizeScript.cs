using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellResizeScript : MonoBehaviour
{
    private GridLayoutGroup _grid;
    [SerializeField] private int totalPadding = 20;
    [SerializeField] private int columnsCount = 3;
    [SerializeField] private float aspectRatio = 1.2f;
    private void OnRectTransformDimensionsChange()
    {
        UpdateCellSize();
    }

    public void UpdateCellSize()
    {
        if (!_grid)
        {
            _grid = GetComponent<GridLayoutGroup>();
        }

        if (_grid)
        {
            var rectTransform = _grid.GetComponent<RectTransform>();
            if (rectTransform)
            {
                var width = rectTransform.rect.width;
                _grid.cellSize = new Vector2((width - totalPadding) / columnsCount, aspectRatio * (width-totalPadding) / columnsCount );
            }
        }
    }
    
}
