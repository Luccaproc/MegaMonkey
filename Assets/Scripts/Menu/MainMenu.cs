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
}
