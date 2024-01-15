using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RunSectionButton : SectionButtonBase
{
    
    //TODO: rename for to checkOwnedLevelsForDisplay
    [SerializeField] private bool checkOwnedLevels = false;
    [SerializeField] private bool checkOwnedBlocks = false;
    [SerializeField] private bool checkSelectedBLocks = false;
    [SerializeField] private bool checkAnyBlocks = false;
    [SerializeField] private bool checkLevelSelected = false;
    [SerializeField] private bool checkRunCompleted = false;
    
    [Space(10)]
    // [SerializeField] private bool checkLevelShopVisitedForHighlight = false;
    [SerializeField] private bool checkBlockShopVisitedForHighlight = false;
    // [SerializeField] private bool checkBriefingVisitedForHighlight = false;
    // [SerializeField] private bool checkBlockSelectionForHighlight = false;
    [SerializeField] private bool checkConfirmationForHighlight = false;

    protected override void Subscribe()
    {
        LevelInventory.Instance.dataChanged += HandleDataChanged;
        BlockInventoryManager.Instance.dataChanged += HandleDataChanged;
        BlockSelectionManager.Instance.dataChanged += HandleDataChanged;
    }

    protected override void Unsubscribe()
    {
        LevelInventory.Instance.dataChanged -= HandleDataChanged;
        BlockInventoryManager.Instance.dataChanged -= HandleDataChanged;
        BlockSelectionManager.Instance.dataChanged -= HandleDataChanged;
    }
    
    protected override bool ShouldBeVisible()
    {
        if (checkOwnedBlocks && !BlockInventoryManager.Instance.HasAnyBlocks())
        {
            return false;
        }

        if (checkOwnedLevels && !LevelInventory.Instance.HasLevels())
        {
            return false;
        }

        if (checkLevelSelected && !BriefingManager.Instance.HasLevelSelected())
        {
            return false;
        }

        if (checkSelectedBLocks && !BlockSelectionManager.Instance.HasBlocks())
        {
            return false;
        }

        if(checkAnyBlocks && !(BlockSelectionManager.Instance.HasBlocks() || BlockInventoryManager.Instance.HasAnyBlocks()))
        {
            return false;
        }

        if(checkRunCompleted && GlobalMapManager.Instance.IsCurrentRunCompleted())
        {
            return false;
        }

        return true;
    }

    protected override bool ShouldBeHighlighted()
    {
        if (checkBlockShopVisitedForHighlight && !BlockShopManager.Instance.GetVisitedCurrentDay())
        {
            return true;
        }

        if (checkConfirmationForHighlight && !ConfirmationManager.Instance.GetVisitedCurrentDay())
        {
            return true;
        }
        
        return false;
    }
}
