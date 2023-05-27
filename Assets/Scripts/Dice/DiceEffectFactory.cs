using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DiceEffectFactory
{
    private int negativeThreshold;
    private int positiveThreshold;

    private System.Func<int, DiceEffect>[] negativeEffectProviders;
    private System.Func<int, DiceEffect>[] neutralEffectProviders;
    private System.Func<int, DiceEffect>[] positiveEffectProviders;

    public DiceEffectFactory(int negativeThreshold, int positiveThreshold)
    {
        this.negativeThreshold = negativeThreshold;
        this.positiveThreshold = positiveThreshold;

        negativeEffectProviders = buildNegativeProviders();
        neutralEffectProviders = buildNeutralProviders();
        positiveEffectProviders = buildPositiveProviders();
    }

    public DiceEffect Generate(int rollResult)
    {
        System.Func<int, DiceEffect>[] providers = rollResult > positiveThreshold ? positiveEffectProviders :
                                                   rollResult < negativeThreshold ? negativeEffectProviders :
                                                   neutralEffectProviders;

        return providers[Random.Range(0, providers.Length)](rollResult);
    }

    private System.Func<int, DiceEffect>[] buildNeutralProviders()
    {
        return new System.Func<int, DiceEffect>[]
        {
            (rollResult) => NeutralStoryEffect.TellHowManyMoreWaves(WaveManager.Instance.RemainingWavesCount),
            (rollResult) => NeutralStoryEffect.DescribeNextWave(WaveManager.Instance.EnemiesInNextWaveCount),
        };
    }

    // TODO handle values of constants
    private System.Func<int, DiceEffect>[] buildNegativeProviders()
    {
        return new System.Func<int, DiceEffect>[]
        {
            (rollResult) => MoneyEffect.Loss(System.Math.Min(GameManager.Instance.Currency, 10)),
            catapultDamageDebuff(-3)
        };
    }

    private System.Func<int, DiceEffect>[] buildPositiveProviders()
    {
        return new System.Func<int, DiceEffect>[]
        {
            (rollResult) => MoneyEffect.Bonus(10),
            catapultDamageBuff(3)
        };
    }

    private List<Tower> getCatapults()
    {
        return GameManager.Instance.GetTowersWithTag(Catapult.TAG);
    }

    private System.Func<int, DiceEffect> catapultDamageDebuff(int val)
    {
        return (rollResult) => TowerStatEffect.Damage(
            getCatapults(),
            val,
            "Our workers became sick and are\n too weak to carry heavy catapult stones.\nCatapult damage for next wave: " + val
        );
    }

    private System.Func<int, DiceEffect> catapultDamageBuff(int val)
    {
        return (rollResult) => TowerStatEffect.Damage(
            getCatapults(),
            val,
            "Our workers found heavier stones for catapults!\nCatapult damage for next wave: +" + val
        );
    }
}
