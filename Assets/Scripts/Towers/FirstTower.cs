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
            new TowerUpgrade(2, 10, 1.5f)
        };
    }


}
