using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    private Util() { }

    public static string FormatShipStatName(string name)
    {
        return string.Format("<color=#ffa500ff><size=20><b>{0}</b></size></color>", name);
    }

    public static string FormatStat(string stat, string upgradeColorHex, string upgrade=null, string operand="+", string unit="")
    {
        return upgrade == null ? string.Format("{0}{1}", stat.ToString(), unit):
                string.Format("{0}{4} <color={1}>{2}{3}{4}</color>", stat, upgradeColorHex, operand, upgrade, unit);
    }
}
