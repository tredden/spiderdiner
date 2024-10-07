using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SceneController : MonoBehaviour
{
    static SceneController instance;

    [SerializeField]
    List<string> levelScenes;

    [SerializeField]
    string mainMenuScene;

    int currentLevel = -1;
    int highestCompletedLevel = -1;

    AudioSource audioSource;

    [SerializeField]
    AudioClip menuMusic;
    [SerializeField]
    AudioClip levelMusic;
    [SerializeField]
    AudioClip bossMusic;

    public static SceneController GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Awake()
    {
        // enforce singleton across scenes
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = this.GetComponent<AudioSource>();
            Scene s = SceneManager.GetActiveScene();
            if (s.name == mainMenuScene) {
                currentLevel = -1;
            } else {
                currentLevel = levelScenes.IndexOf(s.name);
            }
        } else {
            GameObject.Destroy(this);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single);
    }

    public void MarkLevelComplete()
    {
        if (currentLevel > highestCompletedLevel) {
            highestCompletedLevel = currentLevel;
        }
    }

    public void LoadLevel(int level)
    {
        string nextLevel = levelScenes[level];

        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        if (currentLevel == levelScenes.Count - 1) { // final level, go to a full you win
            LoadMainMenu();
        } else {
            this.LoadLevel(currentLevel + 1);
        }
    }

    public bool HasNextLevel()
    {
        return currentLevel < levelScenes.Count - 1;
    }

    public int GetHighestCompletedLevel()
    {
        return highestCompletedLevel;
    }

    public AudioSource GetBackgroundMusicAudioSource()
    {
        return audioSource;
    }

    public void PlayMenuMusic()
    {
        if (audioSource.clip != menuMusic && menuMusic != null) {
            audioSource.clip = menuMusic;
            audioSource.Play();
        }
    }

    public void PlayLevelMusic()
    {
        if (audioSource.clip != levelMusic && levelMusic != null) {
            audioSource.clip = levelMusic;
            audioSource.Play();
        }
    }

    public void PlayBossMusic()
    {
        if (audioSource.clip != bossMusic && bossMusic != null) {
            audioSource.clip = bossMusic;
            audioSource.Play();
        }
    }
}
