using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Number of degrees to rotate in a clockwise direction
/// </summary>
public enum HexRotationDegrees
{
    Zero = 0,
    Sixty = 60,
    OneTwenty = 120,
    OneEighty = 180,
    TwoFourty = 240,
    ThreeHundred = 300,
}

public static class HexRotationDegreesExtension
{
    public static int ToOrdinal(this HexRotationDegrees degrees)
    {
        int index = Array.IndexOf(Enum.GetValues(typeof(HexRotationDegrees)), degrees);
        return index;
    }
}
