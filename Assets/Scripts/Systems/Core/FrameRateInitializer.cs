using UnityEngine;

public class FrameRateInitializer : MonoBehaviour, IInitializable
{
    [SerializeField] private int targetFrameRate = 60;

    public void Initialize()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    private void Awake()
    {
        Initialize();
    }
}