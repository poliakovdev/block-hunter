using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject sensorTop;
    [SerializeField] private GameObject sensorRight;
    [SerializeField] private GameObject sensorBottom;
    [SerializeField] private GameObject sensorLeft;

    public TileType type;
    public TileColor color;

    public Dictionary<Direction, GameObject> sensors;

    public const string MAIN_BLOCK_TAG = "main_block";
    public const string BLOCK_PREFIX = "block_";

    private void Awake()
    {
        sensors = new Dictionary<Direction, GameObject>
        {
            { Direction.Top, sensorTop },
            { Direction.Right, sensorRight },
            { Direction.Bottom, sensorBottom },
            { Direction.Left, sensorLeft }
        };
    }

    public void EnableSensor(Direction direction)
    {
        sensors[direction].GetComponent<Sensor>().Enable();
    }
    public void DesableSensor(Direction direction)
    {
        sensors[direction].GetComponent<Sensor>().Desable();
    }
    public void EnableCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public void DesableCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
