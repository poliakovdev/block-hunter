using UnityEngine;

public class TileCollisionResolver : MonoBehaviour
{
    private TileMovement movement;
    private Vector2 finalPosition;
    public System.Action OnCollected;

    private void Awake()
    {
        movement = GetComponent<TileMovement>();
    }

    public void HandleCollisionBySensorData(GameObject target, Direction direction, TileColor sensorColor)
    {
        var tile = target.GetComponent<Tile>();
        if (tile && tile.type == TileType.Colored && tile.color == sensorColor)
        {
            HandleCollisionWithTarget(target, direction);
        }
        else
        {
            HandleCollisionWithNotTarget(target, direction);
        }
    }

    public void HandleCollisionWithTarget(GameObject target, Direction direction)
    {
        finalPosition = target.GetComponent<RectTransform>().anchoredPosition;
        movement.MoveToPosition(finalPosition, direction);

        OnCollected?.Invoke();

        AudioManager.Instance.PlayFX(AudioManager.tileBreakingFX);
        
        Destroy(target);
    }
    public void HandleCollisionWithNotTarget(GameObject obj, Direction direction)
    {
        
        if (obj.tag == "grid_block")
        {
            finalPosition = obj.GetComponent<RectTransform>().anchoredPosition;
        }
        if (obj.tag != "grid_block")
        {
            movement.Stop();
            movement.MoveToPosition(finalPosition, direction);
        }
    }
}