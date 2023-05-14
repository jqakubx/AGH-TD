using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Building
{
    public override bool CanBeBuiltOn(TileScript tile)
    {
        if (!tile.Walkable) return false;

        var neighbours = LevelManager.Instance.GetNondiagonalNeighboursOf(tile);
        foreach (var neigh in neighbours)
        {
            if (neigh.Attrs.Type == TileAttributes.TileType.SAND ||
               (!neigh.Walkable && neigh.PlacedBuilding != null && neigh.PlacedBuilding is Obstacle))
            { 
                return true;
            }
        }

        return false;
    }

    public override void SetColor(Color newColor)
    {
        GetComponent<SpriteRenderer>().color = newColor;
    }

    public override bool CanBeBuiltDuringWave()
    {
        return false;
    }
}
