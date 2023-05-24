using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralStoryEffect : DiceEffect
{
    private string description;

    public NeutralStoryEffect(string description)
    {
        this.description = description;
    }

    public static NeutralStoryEffect TellHowManyMoreWaves(int waves)
    {
        return new NeutralStoryEffect("Keep going, remaining waves: " + waves + ".");
    }

    public static NeutralStoryEffect DescribeNextWave(int enemyCount)
    {
        return new NeutralStoryEffect("Our scouts discovered that there\n will be " + enemyCount + " enemies in the next wave.");
    }

    public override void ApplyPreWave()
    {
        // don't do anything
    }

    public override string GetDescription()
    {
        return description;
    }
}
