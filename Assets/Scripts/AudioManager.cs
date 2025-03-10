using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance; // Singleton

    public AudioSource audioSource;
    public AudioClip menuMusic;
    public AudioClip levelOneMusic;

    void Start()
    {
        string currentLevel = SceneManager.GetActiveScene().name;

        if (currentLevel == "MainMenu" || currentLevel == "HowToPlay") // Keep menu music playing between help and main menus
        {
            audioSource.clip = menuMusic;
            audioSource.Play();
        }
        else if (currentLevel == "LevelOne")
        {
            audioSource.clip = levelOneMusic;
            audioSource.Play();
        }
    }

    private void Awake() // Singleton
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
