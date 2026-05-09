using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public string soundName = "button_click_fx";

    public void Play()
    {
        AudioManager.Instance.PlayFX(soundName);
    }
}