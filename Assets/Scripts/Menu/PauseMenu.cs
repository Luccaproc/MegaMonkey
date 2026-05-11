using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject Container;

    // 👇 botão que vai ser selecionado ao abrir o pause
    public GameObject firstButton;

    private PlayerControls playerControls;
    public bool isPaused;

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
            if (!isPaused)
                Pause();
            else
                ResumeButton();
        }
    }

    void Pause()
    {
        isPaused = true;

        Container.SetActive(true);
        Time.timeScale = 0f;

        // 👇 força seleção do botão (com delay de 1 frame)
        StartCoroutine(SelectButton());
    }

    private IEnumerator SelectButton()
    {
        yield return null;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void ResumeButton()
    {
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();

        if (player != null)
        {
            player.BlockJumpTemporarily(0.2f);
        }

        isPaused = false;

        Container.SetActive(false);

        Time.timeScale = 1f;
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }

    public void ExitMenu()
    {
        Application.Quit();
    }
}