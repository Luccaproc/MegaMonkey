using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LanguageManager : MonoBehaviour
{
    public Image jogarImage;
    public Image creditosImage;

    public Sprite jogar;
    public Sprite play;

    public Sprite creditos;
    public Sprite credits;

    void Start()
    {
        // Pega idioma salvo (default = PT)
        string language = PlayerPrefs.GetString("Language", "PT");

        if (language == "PT")
        {
            ApplyPortuguese();
        }
        else
        {
            ApplyEnglish();
        }
    }

    public void SetPortuguese()
    {
        PlayerPrefs.SetString("Language", "PT");
        PlayerPrefs.Save();

        ApplyPortuguese();
    }

    public void SetEnglish()
    {
        PlayerPrefs.SetString("Language", "EN");
        PlayerPrefs.Save();

        ApplyEnglish();
    }

    void ApplyPortuguese()
    {
        jogarImage.sprite = jogar;
        creditosImage.sprite = creditos;
    }

    void ApplyEnglish()
    {
        jogarImage.sprite = play;
        creditosImage.sprite = credits;
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}