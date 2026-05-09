using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] public Field field;

    private void Start()
    {
       
        field.Init();
        field.RenderMap();
    }
}