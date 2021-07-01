using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutlineColorHandler
{
    public static List<Color> AvailableOutlineColors { get; private set; }

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
        var color = AvailableOutlineColors.First();
        AvailableOutlineColors.Remove(color);

        return color;
    }

    public static void ReturnOutlineColor(Color color)
    {
        AvailableOutlineColors.Add(color);
    }
}