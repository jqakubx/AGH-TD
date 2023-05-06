using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista : Tower
{
    private new void Start()
    {
        base.Start();
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(6, 4, 0.1f, 1.1f),
            new TowerUpgrade(8, 5, 0.05f, 1.2f),
        };
    }

    public override string GetStats()
    {
        return string.Format("<color=#ffa500ff><size=20><b>Ballista</b></size></color>{0}", base.GetStats());
    }
}
