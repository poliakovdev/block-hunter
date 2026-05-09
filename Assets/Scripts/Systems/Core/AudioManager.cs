using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private Dictionary<string, AudioClip> soundMap;

    public const string moveBlockFX = "block_move_fx";
    public const string tileBreakingFX = "tile_breaking_fx";

    [System.Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip clip;
    }
    public Sound[] sounds;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        soundMap = new Dictionary<string, AudioClip>();

        foreach (var s in sounds)
        {
            soundMap[s.name] = s.clip;
        }

        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    public void PlayFX(string name)
    {
        if (soundMap.ContainsKey(name))
        {
            sfxSource.PlayOneShot(soundMap[name]);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + name);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            FadeOutMusic(musicSource);
        }
        else
        {
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
                FadeInMusic(musicSource);
            }
               
        }
    }

    private void FadeOutMusic(AudioSource music)
    {
        StartCoroutine(FadeOut(music));
    }

    private void FadeInMusic(AudioSource music)
    {
        StartCoroutine(FadeIn(music));
    }

    IEnumerator FadeOut(AudioSource music)
    {
        while (music.volume > 0)
        {
            music.volume -= Time.deltaTime / 5;
            yield return null;
        }

        music.Stop();
    }

    IEnumerator FadeIn(AudioSource music)
    {
        while (music.volume < 1)
        {
            music.volume += Time.deltaTime / 5;
            yield return null;
        }
    }
}
