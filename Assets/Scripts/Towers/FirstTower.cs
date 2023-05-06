using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTower : Tower
{
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(5, 3, 0.15f, 1.15f),
            new TowerUpgrade(10, 5, 0.5f, 1.15f),
        };
    }

    public override string GetStats()
    {
        return string.Format("<color=#ffa500ff><size=20><b>First Tower</b></size></color>{0}", base.GetStats());
    }

}
