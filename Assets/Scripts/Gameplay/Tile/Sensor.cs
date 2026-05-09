using UnityEngine;
using UnityEngine.UI;

public class Sensor : MonoBehaviour
{
    public GameObject parentBlock;
    public TileColor color;
    public Direction direction;

    public void Init(TileColor color)
    {
        this.color = color;
        GetComponent<Image>().color = TileHelper.GetColor32(color);
    }

    public void Enable()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public void Desable()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        parentBlock.GetComponent<TileCollisionResolver>().HandleCollisionBySensorData(collision.gameObject, direction, color);
    }
}
