using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject mainBlock;
    [SerializeField] private GameObject coloredBlock;
    [SerializeField] private GameState gameState;

    [SerializeField] private Sprite graySprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite blackSprite;
    [SerializeField] private Sprite orangeSprite;
    [SerializeField] private Sprite pinkSprite;

    private int blocksCount;
    public System.Action OnColoredBlockSpawned;

    public void SpawnMain(TileData tileData)
    {
        var obj = Instantiate(mainBlock, parent);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(tileData.positionX, tileData.positionY);
       
        var tile = obj.GetComponent<Tile>();
        tile.type = tileData.type;

        var resolver = tile.GetComponent<TileCollisionResolver>();
        resolver.OnCollected += () =>
        {
            gameState.AddScore(1);
        };

        var movement = tile.GetComponent<TileMovement>();
        movement.OnMoveComplete += (startPos,endPos) =>
        {
            gameState.UpdateMap();
            tile.EnableCollider();
            GetComponent<Field>().OnMoveComplete(startPos, endPos);
        };

        foreach (var sensor in tileData.sensors)
        {
            tile.sensors[sensor.Key].GetComponent<Sensor>().Init(sensor.Value);
        }
    }

    public void SpawnColored(TileData tileData)
    {
        Sprite sprite = GetSprite(tileData.color);
        var obj = Instantiate(coloredBlock, parent);

        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(tileData.positionX, tileData.positionY);
        obj.GetComponent<Image>().sprite = sprite;
        obj.GetComponent<Tile>().type = tileData.type;
        obj.GetComponent<Tile>().color = tileData.color;

        OnColoredBlockSpawned?.Invoke();
    }

    private Sprite GetSprite(TileColor color)
    {
        switch (color)
        {
            case TileColor.Gray: return graySprite;
            case TileColor.Blue: return blueSprite;
            case TileColor.Green: return greenSprite;
            case TileColor.Black: return blackSprite;
            case TileColor.Orange: return orangeSprite;
            case TileColor.Pink: return pinkSprite;
            default: return null;
        }
    }
}