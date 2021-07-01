using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class OutlineColorHandler
{
    private static List<Color> AvailableOutlineColors { get; }

    static OutlineColorHandler()
    {
        AvailableOutlineColors = new List<Color>()
        {
            Color.blue,
            Color.green,
            Color.red,
            Color.yellow,
            Color.black,
            Color.white,
            Color.cyan,
            Color.magenta
        };
    }

    public static Color GetOutlineColor()
    {
        var color = AvailableOutlineColors.FirstOrDefault();
        if (color != null)
        {
            AvailableOutlineColors.Remove(color);
        }

        return color;
    }

    public static void ReturnOutlineColor(Color color)
    {
        AvailableOutlineColors.Add(color);
    }
}