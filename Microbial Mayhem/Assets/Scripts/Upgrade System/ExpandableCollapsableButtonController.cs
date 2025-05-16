using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandableCollapsableButtonController : MonoBehaviour
{
    public GameObject CollapsedUpgradePanel;
    public GameObject ExpandedUpgradePanel;

    public void CollapseUpgradePanel()
    {
        ExpandedUpgradePanel.SetActive(false);
        CollapsedUpgradePanel.SetActive(true);
    }

    public void ExpandUpgradePanel()
    {
        CollapsedUpgradePanel.SetActive(false);
        ExpandedUpgradePanel.SetActive(true);
    }
}
