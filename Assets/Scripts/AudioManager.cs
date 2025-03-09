using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip menuMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
