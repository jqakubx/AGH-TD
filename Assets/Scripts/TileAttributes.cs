using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAttributes
{
    public enum TileType
    {
        SAND = 0,
        WATER = 1
    }

    public TileType Type { get; }
    public bool Walkable { get; }
    public bool Buildable { get; }
    public int PrefabIdx { get; }

    public Color32 FullColor { get; }
    public Color32 EmptyColor { get; }

    private TileAttributes(TileType tileType, bool walkable, bool buildable, Color32 fullColor, Color32 emptyColor)
    {
        this.Type = tileType;
        this.Walkable = walkable;
        this.Buildable = buildable;
        this.PrefabIdx = (int)tileType;
        this.FullColor = fullColor;
        this.EmptyColor = emptyColor;
    }

    public static TileAttributes FromStringRepr(string tileType)
    {
        switch ((TileType) int.Parse(tileType))
        {
            case TileType.SAND:
                return new TileAttributes(TileType.SAND, false, true, new Color32(255, 118, 118, 255), new Color32(96, 255, 90, 255));
            case TileType.WATER:
                return new TileAttributes(TileType.WATER, true, false, Color.white, Color.white);
            default:
                throw new System.ArgumentException("Invalid tile idx " + tileType);
        }
    }
}