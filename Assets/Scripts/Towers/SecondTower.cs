using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondTower : Tower
{
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(5, 2, 0.15f),
            new TowerUpgrade(10, 3, 0.2f),
        };
    }

    public override string GetStats()
    {
        return string.Format("<color=#ffa500ff><size=20><b>Second Tower</b></size></color>{0}", base.GetStats());
    }
}
