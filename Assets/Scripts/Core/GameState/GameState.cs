using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    [SerializeField] private Text pointsText;
    [SerializeField] private Text bestResultText;

    public int score;
    public int bestResult;
    public int difficultyLevel;

    public void Awake()
    {
        difficultyLevel = PlayerStorage.GetDifficulty();

        score = PlayerStorage.GetPoints();
        PrintPoints();

        bestResult = PlayerStorage.GetBestResult();
        PrintBestResult();
    }

    public void AddScore(int value)
    {
        score += value;
        if (score > bestResult)
        {
            bestResult = score;
            PlayerStorage.UpdateBestResult(bestResult, difficultyLevel);
            PrintBestResult();
        }

        PlayerStorage.UpdatePoints(score);
        PrintPoints();
    }

    public void OnTileStopped()
    {
        UpdateMap();
    }

    public void OnTileCreated()
    {
        UpdateMap();
    }

    public void UpdateMap()
    {
        Tile[] tiles = FindObjectsByType<Tile>();
        PlayerStorage.UpdateMap(tiles);
    }

    private void PrintPoints()
    {
        pointsText.GetComponent<Text>().text = score.ToString();
    }

    private void PrintBestResult()
    {
        bestResultText.GetComponent<Text>().text = bestResult.ToString();
    }
}