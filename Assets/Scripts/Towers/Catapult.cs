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

    public override string GetStats()
    {
        return string.Format("<color=#ffa500ff><size=20><b>Catapult</b></size></color>{0}", base.GetStats());
    }
}
