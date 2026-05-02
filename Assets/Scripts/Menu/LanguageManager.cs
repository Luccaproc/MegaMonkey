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

    public void SetPortuguese()
    {
        PlayerPrefs.SetString("Language", "PT");

        jogarImage.sprite = jogar;
        creditosImage.sprite = creditos;
    }

    public void SetEnglish()
    {
        PlayerPrefs.SetString("Language", "EN");

        jogarImage.sprite = play;
        creditosImage.sprite = credits;
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}