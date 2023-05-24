using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiceEffect
{
    public abstract void ApplyPreWave();

    public virtual void ApplyPostWave()
    {
        // do nothing by default
    }

    public abstract string GetDescription(); 
}
