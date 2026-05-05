using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject Container;
    private PlayerControls playerControls;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        if (playerControls.Player.Menu.WasPressedThisFrame())
        {
            Container.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeButton()
    {
        Container.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1; // importante resetar antes de trocar cena
        SceneManager.LoadSceneAsync(0);
    }

    public void ExitMenu()
    {
        Application.Quit();
    }
}