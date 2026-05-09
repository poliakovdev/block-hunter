using UnityEngine;
using UnityEngine.EventSystems;

public class TileInput : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    private TileMovement movement;
    private Tile tile;

    private float startX;
    private float startY;

    private void Awake()
    {
        movement = GetComponent<TileMovement>();
        tile = GetComponent<Tile>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startX = Input.mousePosition.x;
        startY = Input.mousePosition.y;

        movement.SetStartPosition();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float dx = Mathf.Abs(startX - Input.mousePosition.x);
        float dy = Mathf.Abs(startY - Input.mousePosition.y);
        if (dx < 5 && dy < 5) return;

        Direction direction = dx > dy
            ? (Input.mousePosition.x < startX ? Direction.Left : Direction.Right)
            : (Input.mousePosition.y < startY ? Direction.Bottom : Direction.Top);

        tile.EnableSensor(direction);
        tile.DesableCollider();
        AudioManager.Instance.PlayFX(AudioManager.moveBlockFX);
        movement.Move(direction);
    }

    public void OnDrag(PointerEventData eventData)
    {  
    }
}