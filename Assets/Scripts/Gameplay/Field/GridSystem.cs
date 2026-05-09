using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public const int STATUS_AVAILABLE = 0;
    public const int STATUS_FORBIDDEN = 1;
    public const int MAX_POSITIONS_COUNT = 36;

    public class PositionData
    {
        public int status;
        public TileType type;
        public TileColor color;

        public PositionData(int status)
        {
            this.status = status;
        }

        public PositionData(int status, TileType type)
        {
            this.status = status;
            this.type = type;
        }

        public PositionData(int status, TileType type, TileColor color)
        {
            this.status = status;
            this.type = type;
            this.color = color;
        }
    }

    private Dictionary<string, PositionData> positionsData;

    const int MIN_LEFT_POSITION = -400;
    const int MAX_RIGHT_POSITION = 400;
    const int MIN_BOTTOM_POSITION = 110;
    const int MAX_TOP_POSITION = 910;
    const int STEP = 160;

    public void Init()
    {
        positionsData = new Dictionary<string, PositionData>();

        for (int x = MIN_LEFT_POSITION; x <= MAX_RIGHT_POSITION; x += STEP)
        {
            for (int y = MIN_BOTTOM_POSITION; y <= MAX_TOP_POSITION; y += STEP)
            {
                positionsData[x + "," + y] = new PositionData(STATUS_AVAILABLE);
            }
        }
    }

    public Dictionary<string, PositionData> GetData()
    {
        return positionsData;
    }

    public PositionData GetPositionData(Vector2 pos)
    {
        string key = pos.x + "," + pos.y;
        return positionsData[key];
    }

    public bool isIssetPosition(Vector2 pos)
    {
        string key = pos.x + "," + pos.y;
        return positionsData.ContainsKey(key);
    }

    public Vector2 GetRandomAvailablePosition()
    {
        List<string> availablePositions = new List<string>();

        foreach (KeyValuePair<string, PositionData> item in positionsData)
        {
            if (item.Value.status == STATUS_AVAILABLE)
                availablePositions.Add(item.Key);
        }

        if (availablePositions.Count == 0)
        {
            Debug.LogError("No available positions!");
            return Vector2.zero;
        }

        string pos = availablePositions[UnityEngine.Random.Range(0, availablePositions.Count)];
        string[] arr = pos.Split(',');

        return new Vector2(int.Parse(arr[0]), int.Parse(arr[1]));
    }

    public void SetForbidden(Vector2 pos, TileType type)
    {
        string key = pos.x + "," + pos.y;
        positionsData[key] = new PositionData(STATUS_FORBIDDEN, type);
    }

    public void SetForbidden(Vector2 pos, TileType type, TileColor color)
    {
        string key = pos.x + "," + pos.y;
        positionsData[key] = new PositionData(STATUS_FORBIDDEN, type, color);
    }

    public void ClearForbidden(Vector2 pos)
    {
        string key = pos.x + "," + pos.y;
        positionsData[key] = new PositionData(STATUS_AVAILABLE);
    }

    public bool IsAvailablePosition(Vector2 pos)
    {
        string key = pos.x + "," + pos.y;
        return positionsData.ContainsKey(key) && positionsData[key].status == STATUS_AVAILABLE;
    }

    public bool IsBlockMovementAvailable()
    {
        GameObject[] mainBlocks = GameObject.FindGameObjectsWithTag(Tile.MAIN_BLOCK_TAG);
        foreach (GameObject block in mainBlocks) { 
            if (CheckBlockAvailablePosition(block))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckBlockAvailablePosition(GameObject block)
    {
        Vector2 pos = block.GetComponent<RectTransform>().anchoredPosition;
        Tile tile = block.GetComponent<Tile>();

        return CheckDirection(pos, tile, Direction.Left, -STEP, 0) ||
               CheckDirection(pos, tile, Direction.Right, STEP, 0) ||
               CheckDirection(pos, tile, Direction.Top, 0, STEP) ||
               CheckDirection(pos, tile, Direction.Bottom, 0, -STEP);
    }

    private bool CheckDirection(Vector2 pos, Tile tile, Direction dir, int offsetX, int offsetY)
    {
        Vector2 newPos = new Vector2(pos.x + offsetX, pos.y + offsetY);

        if (!isIssetPosition(newPos))
        {
            return false;
        }
           

        if (IsAvailablePosition(newPos))
        {
            return true;
        }

        PositionData data = GetPositionData(newPos);
        GameObject sensorObj = tile.sensors[dir];
        Sensor sensor = sensorObj.GetComponent<Sensor>();

        return data.type == TileType.Colored && sensor.color == data.color;
    }

    private bool CheckBlockAvailablePosition2(GameObject block)
    {
        Vector2 blockPosition = block.GetComponent<RectTransform>().anchoredPosition;

        Vector2 leftBlockPosition = new Vector2(blockPosition.x - STEP, blockPosition.y);
        if (isIssetPosition(leftBlockPosition))
        {
            if(IsAvailablePosition(leftBlockPosition))
            {
                return true;
            }

            PositionData leftBlock = GetPositionData(leftBlockPosition);
            GameObject leftSensor = block.GetComponent<Tile>().sensors[Direction.Left];
            if (leftBlock.type == TileType.Colored && leftSensor.GetComponent<Sensor>().color == leftBlock.color)
            {
                return true;
            }
        }

        Vector2 rightBlockPosition = new Vector2(blockPosition.x + STEP, blockPosition.y);
        if (isIssetPosition(rightBlockPosition))
        {
            if (IsAvailablePosition(rightBlockPosition))
            {
                return true;
            }

            PositionData rightBlock = GetPositionData(rightBlockPosition);
            GameObject rightSensor = block.GetComponent<Tile>().sensors[Direction.Right];
            if (rightBlock.type == TileType.Colored && rightSensor.GetComponent<Sensor>().color == rightBlock.color)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsNearestBlockIsTarget(GameObject block)
    {
        return false;
    }
}