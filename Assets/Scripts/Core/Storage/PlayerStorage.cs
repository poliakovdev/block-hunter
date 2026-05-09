
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class PlayerStorage
{
    public static void UpdatePoints(int points)
    {
        PlayerPrefs.SetInt("points", points);
    }
    public static void UpdateBestResult(int bestRsult, int difficulty)
    {
        
        PlayerPrefs.SetInt("best_result_" + difficulty, bestRsult);
    }

    public static int GetPoints()
    {
        return PlayerPrefs.GetInt("points");
    }

    public static int GetBestResult()
    {
        int difficulty = PlayerPrefs.GetInt("difficulty");
        return PlayerPrefs.HasKey("best_result_" + difficulty) ? PlayerPrefs.GetInt("best_result_" + difficulty) : 0;
    }
    public static int GetDifficulty()
    {
        return PlayerPrefs.GetInt("difficulty");
    }

    public static bool IsMapIsset()
    {
        return PlayerPrefs.HasKey("map") &&
               !string.IsNullOrEmpty(PlayerPrefs.GetString("map"));
    }

    public static List<TileData> GetMap()
    {
        if (PlayerPrefs.HasKey("map") && PlayerPrefs.GetString("map") != "")
        {
            var mapStr = PlayerPrefs.GetString("map");
            return JsonConvert.DeserializeObject<List<TileData>>(mapStr);
        }
       
        return new List<TileData>();
    }

    public static void ResetMap()
    {
        PlayerPrefs.SetString("map", "");
    }

    public static void UpdateMap(Tile[] tiles)
    {
        List<TileData> dataList = new List<TileData>();

        foreach (Tile tile in tiles)
        {
            TileData data = new TileData();
            data.type = tile.type;
            data.positionX = (int)tile.GetComponent<RectTransform>().anchoredPosition.x;
            data.positionY = (int)tile.GetComponent<RectTransform>().anchoredPosition.y;

            if (tile.type == TileType.Main)
            {
                data.sensors = new Dictionary<Direction, TileColor>();
                foreach (KeyValuePair<Direction, GameObject> sensor in tile.sensors)
                {
                    data.sensors.Add(sensor.Key, sensor.Value.GetComponent<Sensor>().color);
                }
            }
            if (tile.type == TileType.Colored)
            {
                data.color = tile.color;
            }

            dataList.Add(data);
        }

        PlayerPrefs.SetString("map", JsonConvert.SerializeObject(dataList));
    }
}






