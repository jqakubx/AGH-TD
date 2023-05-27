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
            new TowerUpgrade(9, 4, 0.15f, 1.1f),
            new TowerUpgrade(13, 5, 0.1f, 1.2f),
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
