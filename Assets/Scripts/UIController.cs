using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.Audio;

public class UIController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSFX;
    public AudioClip whooshSFX;

    public void StartGame()
    {
        StartCoroutine(SoundAndScene(whooshSFX, "LevelOne")); // Passing in sound and name of level we're advancing to
    }

    public void HelpMenu()
    {
        StartCoroutine(SoundAndScene(clickSFX, "HowToPlay"));
    }

    public void MainMenu()
    {
        StartCoroutine(SoundAndScene(clickSFX, "MainMenu"));
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator SoundAndScene(AudioClip clip, string scene) // YTakes clip name and scene name
    {
        audioSource.PlayOneShot(clip); // Play sound
        yield return new WaitForSeconds(clip.length); // Finish sound

        SceneManager.LoadScene(scene); // Load new scene
    }
}
