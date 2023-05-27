using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyEffect : DiceEffect
{
    private int moneyChange;
    private string description;

    private MoneyEffect(int moneyChange, string description)
    {
        this.moneyChange = moneyChange;
        this.description = description;
    }

    public static MoneyEffect Bonus(int value)
    {
        return new MoneyEffect(value, "Our workers found a gold ore!\n You receive + " + value + " gold!");
    }

    public static MoneyEffect Loss(int value)
    {
        return new MoneyEffect(-value, "There was a mutiny on one of islands!\n You had to pay " + value + " to workers to keep them obedient.");
    }

    public override void ApplyPreWave()
    {
        GameManager.Instance.Currency += moneyChange;
    }

    public override string GetDescription()
    {
        return description;
    }

}
