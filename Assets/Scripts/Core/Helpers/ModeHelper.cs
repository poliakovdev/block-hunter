using System.Collections.Generic;
public static class ModeHelper
{
    private static readonly TileColor[] allColors =
    {
        TileColor.Blue,
        TileColor.Green,
        TileColor.Black,
        TileColor.Orange,
        TileColor.Pink,
        TileColor.Gray,
    };

    public static List<TileColor> GetColors()
    {
        int difficulty = PlayerStorage.GetDifficulty();

        List<TileColor> result = new List<TileColor>();

        int count = difficulty * 2;

        for (int i = 0; i < count; i++)
        {
            result.Add(allColors[i]);
        }

        return result;
    }
}
