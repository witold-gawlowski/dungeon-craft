using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DragManagerScript : MonoBehaviour
{
    private GameObject draggedBlock;
    private Vector2 offset;
    private Vector3 mousePosition;
    private bool isDraggedBlockSnapped;
    private float dragDuration;
    private Vector3 mouseTouchStartPosition;
    private bool dragStarted;
    
    public Action<GameObject> freeBlockDragStart;

    public static DragManagerScript Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        isDraggedBlockSnapped = false;
        dragStarted = false;
    }
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D collider = Physics2D.OverlapPoint(mousePosition);
                if (collider)
                {
                    draggedBlock = collider.transform.parent.gameObject;
                    offset = draggedBlock.transform.position - mousePosition;
                    dragDuration = 0;
                    mouseTouchStartPosition = mousePosition;
                }
            }

            if (draggedBlock)
            {
                if (!IsTap() && !dragStarted)
                {
                    OnStartDrag();
                    dragStarted = true;
                }

                if (dragStarted)
                {
                    OnDrag();
                }
            }
        }
        
        if (draggedBlock && Input.GetMouseButtonUp(0))
        {
            if (IsTap())
            {
                HandleTap();
            }

            if (dragStarted)
            {
                FinishDrag();
                dragStarted = false;
            }
            
            draggedBlock = null;
        }
    }

    private bool IsTap()
    {
        var mag = (mousePosition - mouseTouchStartPosition).magnitude;
        return mag < 0.15f && dragDuration < 0.15f;
    }

    private void OnStartDrag()
    {
        if (MapManager.GetInstance().IsPlaced(draggedBlock))
        {
            MapManager.GetInstance().Remove(draggedBlock);
            SpawnManager.Instance.HandleBLockUnsnapped(draggedBlock);
            UpdateBlockColor(draggedBlock, false);
        }
        else
        {
            freeBlockDragStart(draggedBlock);
        }
    }
    
    private void OnDrag()
    {
        UpdatePosition();

        dragDuration += Time.deltaTime;
        return;

        void UpdatePosition()
        {
            var newPosition = (Vector2) mousePosition + offset;
            var newPositionRounded = Helpers.RoundPosition(newPosition);
            if (MapManager.GetInstance().CanBePlaced(draggedBlock, newPositionRounded))
            {
                draggedBlock.transform.position = new Vector3(newPositionRounded.x, newPositionRounded.y);
                isDraggedBlockSnapped = true;
                return;
            }
            draggedBlock.transform.position = newPosition;
            isDraggedBlockSnapped = false;
        }
    }

    private void FinishDrag()
    {
        if (isDraggedBlockSnapped)
        {
            var coords = Helpers.RoundPosition(draggedBlock.transform.position);
            MapManager.GetInstance().Place(draggedBlock, coords);
            SpawnManager.Instance.HandleBlockSnapped(draggedBlock);
            UpdateBlockColor(draggedBlock, true);
        }
        else
        {
            SpawnManager.Instance.HandleBlockDragEndedWhenNotSnapped(draggedBlock);
        }
    }

    private void UpdateBlockColor(GameObject block, bool snapped)
    {
        BlockScript script = block.GetComponent<BlockScript>();
        if (script)
        {
            if (snapped)
            {
                script.SetToSnappedColor();
            }
            else
            {
                script.SetToFreeColor();
            }
        }
    }

    private void HandleTap()
    {
        draggedBlock.transform.Rotate(Vector3.forward, 90);
    }
}
