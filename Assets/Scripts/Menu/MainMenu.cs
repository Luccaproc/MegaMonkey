using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource clickSound;
    public void PlayClickSound()
    {
        clickSound.Play();
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void PlayCredits()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void Info()
    {
        SceneManager.LoadSceneAsync(4);
    }
    public void Menu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
