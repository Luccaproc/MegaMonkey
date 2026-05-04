using UnityEngine;

public class CreditsLanguage : MonoBehaviour
{
    public GameObject creditosPortugues;
    public GameObject creditsEnglish;

    void Start()
    {
        string language = PlayerPrefs.GetString("Language", "PT");

        bool isPortuguese = language == "PT";

        creditosPortugues.SetActive(isPortuguese);
        creditsEnglish.SetActive(!isPortuguese);
    }
}