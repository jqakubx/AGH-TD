using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : BuildingButton
{
    public void ShowInfo()
    { 
        Tower tower = BuildingPrefab.GetComponentInChildren<Tower>();
        GameManager.Instance.SetTooltipText(tower.GetTooltipInfo());
        GameManager.Instance.ShowStats();
    }

    public override void OnBuildingPicked()
    {
        Hover.Instance.Activate(Sprite, ((Tower) BuildingTemplate).Range);
    }
}
