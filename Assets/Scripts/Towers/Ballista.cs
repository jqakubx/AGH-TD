using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista : Tower
{
    public static readonly string TAG = "ballista";

    private new void Start()
    {
        base.Start();
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(6, 4, 0.1f, 1.1f),
            new TowerUpgrade(8, 5, 0.05f, 1.2f),
        };
    }


    protected override string GetTowerName()
    {
        return "Ballista";
    }

    protected override string GetDescription()
    {
        return "Medium range and damage, quite fast";
    }

    public override string GetTag()
    {
        return TAG;
    }
}
