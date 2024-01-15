using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSectionButton : SectionButtonBase
{
    [SerializeField] private bool checkRunSelected = false;

    protected override void Subscribe()
    {
        GlobalMapManager.Instance.datachanged += HandleDataChanged;
    }

    protected override void Unsubscribe()
    {
        GlobalMapManager.Instance.datachanged -= HandleDataChanged;
    }

    protected override bool ShouldBeVisible()
    {
        var runConfig = GlobalMapManager.Instance.GetSelectedRunConfig();
        if (checkRunSelected)
        {
            if (runConfig == null || !runConfig.IsValid())
            {
                return false;
            }
        }
        return true;
    }
}
