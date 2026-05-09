using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHelper : MonoBehaviour
{
    public static string GenerateRandomColor()
    {
        List<TileColor> colors = ModeHelper.GetColors();
        int index = Random.Range(0, colors.Count);

        return "red";
        //return colors[index];
    }

    public static Color32 GetColor32(TileColor color)
    {
        return colors[color];
    }

    private static readonly Dictionary<TileColor, Color32> colors = new()
    {
        { TileColor.Green, new Color32(0, 188, 63, 255) },
        { TileColor.Blue, new Color32(0, 216, 255, 255) },
        { TileColor.Gray, new Color32(196, 196, 196, 255) },
        { TileColor.Black, new Color32(0, 0, 0, 255) },
        { TileColor.Orange, new Color32(255, 127, 0, 255) },
        { TileColor.Pink, new Color32(211, 0, 201, 255) },
    };
}
