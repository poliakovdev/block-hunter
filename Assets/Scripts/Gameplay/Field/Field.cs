using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;


public class Field : MonoBehaviour
{
    [SerializeField] private GridSystem grid;
    [SerializeField] private TileSpawner spawner;
    [SerializeField] private GameState state;

    public GameObject gameOverText;

    private List<TileColor> colors;

    public bool coloredSpawned;
    public const int START_COLORED_BLOCKS_COUNT = 10;

    public void Init()
    {
        colors = ModeHelper.GetColors();
        grid.Init();
    }

    public void RenderMap()
    {
        var map = PlayerStorage.GetMap();
        if (map == null || map.Count == 0)
        {
            RenderRandomMap();
            Tile[] tiles = FindObjectsByType<Tile>();
            PlayerStorage.UpdateMap(tiles);
            return;
        }

        foreach (var tile in map)
        {
            if (tile.type == TileType.Main)
            {
                spawner.SpawnMain(tile);
                grid.SetForbidden(new Vector2(tile.positionX, tile.positionY), tile.type);
            }

            if (tile.type == TileType.Colored)
            {
                spawner.SpawnColored(tile);
                grid.SetForbidden(new Vector2(tile.positionX, tile.positionY), tile.type, tile.color);
            }
        }
    }

    private void RenderRandomMap()
    {
        int colorsQuantity = colors.Count;

        List<TileColor> topColors = new List<TileColor>();
        List<TileColor> rightColors = new List<TileColor>();
        List<TileColor> bottomColors = new List<TileColor>();
        List<TileColor> leftColors = new List<TileColor>();

        for (int i = 0; i < colorsQuantity; i++)
        {
            Vector2 position = grid.GetRandomAvailablePosition();

            var sensors = new Dictionary<Direction, TileColor>
            {
                { Direction.Top, GetSensorRandomColor(colors, topColors) },
                { Direction.Right, GetSensorRandomColor(colors, rightColors) },
                { Direction.Bottom, GetSensorRandomColor(colors, bottomColors) },
                { Direction.Left, GetSensorRandomColor(colors, leftColors) }
            };

            TileData mainTile = new TileData();
            mainTile.type = TileType.Main;
            mainTile.positionX = (int)position.x;
            mainTile.positionY = (int)position.y;
            mainTile.sensors = sensors;
            
            spawner.SpawnMain(mainTile);
            grid.SetForbidden(position, TileType.Main);
        }
        
        for (int i = 0; i < START_COLORED_BLOCKS_COUNT; i++)
        {
            CreateRandomColoredTile();
        }
        coloredSpawned = true;
    }

    public void CreateRandomColoredTile()
    {
        Vector2 position = grid.GetRandomAvailablePosition();

        TileData coloredTile = new TileData();
        coloredTile.type = TileType.Colored;
        coloredTile.positionX = (int)position.x;
        coloredTile.positionY = (int)position.y;
        coloredTile.color = colors[Random.Range(0, colors.Count)];

        spawner.SpawnColored(coloredTile);
        grid.SetForbidden(position, TileType.Colored, coloredTile.color);

        if (!grid.IsBlockMovementAvailable())
        {
            gameOverText.SetActive(true);
            PlayerStorage.ResetMap();
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    private TileColor GetSensorRandomColor(List<TileColor> colors, List<TileColor> usedColors)
    {
        TileColor randomColor;

        randomColor = colors[Random.Range(0, colors.Count)];
        if (usedColors.Contains(randomColor))
        {
            return GetSensorRandomColor(colors, usedColors);
        }

        usedColors.Add(randomColor);
        return randomColor;
    }

    public void OnMoveComplete(Vector2 startPos, Vector2 endPos)
    {
        grid.ClearForbidden(startPos);
        grid.SetForbidden(endPos, TileType.Main);
        if (coloredSpawned)
        {
            coloredSpawned = false;
        } else
        {
            coloredSpawned = true;
            CreateRandomColoredTile();
        }
    }
}