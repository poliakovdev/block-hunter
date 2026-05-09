using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public TileType type;
    public int positionX;
    public int positionY;
    public TileColor color;
    public Dictionary<Direction, TileColor> sensors;
}