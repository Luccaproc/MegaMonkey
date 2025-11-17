using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour
{
    public AudioSource gameMusic;
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
