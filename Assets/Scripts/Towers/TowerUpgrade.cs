using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade
{
    public int Price { get; private set; }
    
    public int Damage { get; private set; }
    
    public float Cooldown { get; private set; }

    public TowerUpgrade(int price, int damage, float cooldown)
    {
        this.Price = price;
        this.Damage = damage;
        this.Cooldown = cooldown;
    }
}
