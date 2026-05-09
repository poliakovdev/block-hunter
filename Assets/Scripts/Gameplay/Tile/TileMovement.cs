using UnityEngine;

public class TileMovement : MonoBehaviour
{
    private RectTransform rect;
    private Rigidbody2D rb;

    public const float speed = 3f;

    private Vector2 startPosition;
    private Vector2 endPosition;

    public System.Action<Vector2, Vector2> OnMoveComplete;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetStartPosition()
    {
        startPosition = rect.anchoredPosition;
    }

    public void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                rb.linearVelocity = new Vector2(-speed, 0);
                break;
            case Direction.Right:
                rb.linearVelocity = new Vector2(speed, 0);
                break;
            case Direction.Top:
                rb.linearVelocity = new Vector2(0, speed);
                break;
            case Direction.Bottom:
                rb.linearVelocity = new Vector2(0, -speed);
                break;
        }
    }

    public void Stop()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void MoveToPosition(Vector2 position, Direction direction)
    {
        endPosition = position;
        var currentPos = rect.anchoredPosition;
        
        if (direction == Direction.Top || direction == Direction.Bottom)
        {
            rect.anchoredPosition = new Vector2(endPosition.x, currentPos.y);
            if (currentPos.y < endPosition.y)
            {
                rb.linearVelocity = new Vector2(0, speed);
                InvokeRepeating("StopOnTopPosition", 0, 0.01f);
            }
            if (currentPos.y > endPosition.y)
            {
                rb.linearVelocity = new Vector2(0, -speed);
                InvokeRepeating("StopOnBottomPosition", 0, 0.01f);
            }
            return;
        }

        rect.anchoredPosition = new Vector2(currentPos.x, endPosition.y);
        if (currentPos.x < endPosition.x)
        {
            rb.linearVelocity = new Vector2(speed, 0);
            InvokeRepeating("StopOnRightPosition", 0, 0.01f);
        }
        if (currentPos.x > endPosition.x)
        {
            rb.linearVelocity = new Vector2(-speed, 0);
            InvokeRepeating("StopOnLeftPosition", 0, 0.01f);
        }

    }

    public void StopOnTopPosition()
    {
       if (rect.anchoredPosition.y > endPosition.y)
        {
            Stop();
            CancelInvoke();
            rect.anchoredPosition = endPosition;

            OnMoveComplete?.Invoke(startPosition, endPosition);
        }
    }
    public void StopOnBottomPosition()
    {
        if (rect.anchoredPosition.y <= endPosition.y)
        {
            Stop();
            CancelInvoke();
            rect.anchoredPosition = endPosition;

            OnMoveComplete?.Invoke(startPosition, endPosition);
        }
    }
    public void StopOnRightPosition()
    {
        if (rect.anchoredPosition.x > endPosition.x)
        {
            Stop();
            CancelInvoke();
            rect.anchoredPosition = endPosition;

            OnMoveComplete?.Invoke(startPosition, endPosition);
        }
    }
    public void StopOnLeftPosition()
    {
        if (rect.anchoredPosition.x < endPosition.x)
        {
            Stop();
            CancelInvoke();
            rect.anchoredPosition = endPosition;

            OnMoveComplete?.Invoke(startPosition, endPosition);
        }
    }
}