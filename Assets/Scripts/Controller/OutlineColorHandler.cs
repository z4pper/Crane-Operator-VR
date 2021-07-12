using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class OutlineColorHandler
{
    private static readonly List<Color> AvailableOutlineColors;

    static OutlineColorHandler()
    {
        AvailableOutlineColors = new List<Color>()
        {
            Color.blue,
            Color.green,
            Color.red,
            Color.yellow,
            Color.black,
            Color.cyan,
            Color.magenta
        };
    }

    public static Color GetOutlineColor()
    {
        if (AvailableOutlineColors.Count == 0) return new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f), 1f);
        
        var color = AvailableOutlineColors.First();

        AvailableOutlineColors.Remove(color);
        
        return color;
    }

    public static void ReturnOutlineColor(Color color)
    {
        if(!AvailableOutlineColors.Contains(color)) AvailableOutlineColors.Add(color);
    }
}