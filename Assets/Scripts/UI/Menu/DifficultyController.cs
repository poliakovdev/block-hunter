using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyController : MonoBehaviour
{
    private const string GameSceneName = "Game";

    public void SetEasy()
    {
        SetDifficulty(1);
    }

    public void SetMedium()
    {
        SetDifficulty(2);
    }

    public void SetHard()
    {
        SetDifficulty(3);
    }

    private void SetDifficulty(int value)
    {
        PlayerPrefs.SetString("map", "");
        PlayerPrefs.SetInt("points", 0);
        PlayerPrefs.SetInt("difficulty", value);
        SceneManager.LoadScene("Game");
    }
}