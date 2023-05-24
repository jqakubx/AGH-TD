using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStatEffect : DiceEffect
{
    private List<Tower> towers;
    private System.Action<Tower> preWaveModifier;
    private System.Action<Tower> postWaveModifier;
    private string effectDescription;

    public TowerStatEffect(List<Tower> towers, Action<Tower> preWaveModifier, Action<Tower> postWaveModifier, string effectDescription)
    {
        this.towers = towers;
        this.preWaveModifier = preWaveModifier;
        this.postWaveModifier = postWaveModifier;
        this.effectDescription = effectDescription;
    }

    public static TowerStatEffect Damage(List<Tower> towers, int damageDelta, string effectDescription)
    {
        return new TowerStatEffect(
            towers,
            tower => tower.Damage += damageDelta,
            tower => tower.Damage -= damageDelta,
            effectDescription
       );
    }

    public override void ApplyPreWave()
    {
        towers.ForEach(preWaveModifier);
    }

    public override void ApplyPostWave()
    {
        towers.ForEach(postWaveModifier);
    }

    public override string GetDescription()
    {
        return effectDescription;
    }
}
