using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Tower
{
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(8, 5, 0.15f, 1.1f),
            new TowerUpgrade(12, 6, 0.1f, 1.1f),
        };
    }

    protected override string GetTowerName()
    {
        return "Catapult";
    }

    protected override string GetDescription()
    {
        return "High range and damage but slow";
    }
}
