using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Point
{
    public int X { get; set; }

    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public static bool operator ==(Point x, Point y)
    {
        return x.X == y.X && x.Y == y.Y;
    }

    public static bool operator !=(Point x, Point y)
    {
        return x.X != y.X || x.Y != y.Y;
    }

    public static Point operator -(Point x, Point y)
    {
        return new Point(x.X - y.X, x.Y - y.Y);
    }

    public override int GetHashCode()
    {
        return Tuple.Create(X, Y).GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is Point)
        {
            Point p = (Point)obj;
            return X == p.X & Y == p.Y;
        }
        else
        {
            return false;
        }
    }
}
