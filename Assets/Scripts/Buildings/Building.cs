using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public int Price { get; set; }

    public virtual void OnClick()
    {
        GameManager.Instance.DeselectSelection();
    }

    public abstract bool CanBeBuiltOn(TileScript tile);

    public abstract bool CanBeBuiltDuringWave();

    public abstract void SetColor(Color newColor);

    public abstract string GetTag();
}
