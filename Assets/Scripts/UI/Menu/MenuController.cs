using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject continueButton;

    private void Start()
    {
        if (!PlayerStorage.IsMapIsset())
        {
            continueButton.SetActive(false);
        }
    }

    public void OnNewGameClick()
    {
        SceneManager.LoadScene("Difficulty");
    }

    public void OnContinueClick()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}