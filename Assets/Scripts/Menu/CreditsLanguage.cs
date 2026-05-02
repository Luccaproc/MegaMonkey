using UnityEngine;

public class CreditsLanguage : MonoBehaviour
{
    public GameObject creditosPortugues;
    public GameObject creditsEnglish;

    void Start()
    {
        string language = PlayerPrefs.GetString("Language", "PT");

        if (language == "PT")
        {
            creditosPortugues.SetActive(true);
            creditsEnglish.SetActive(false);
        }
        else if (language == "EN")
        {
            creditosPortugues.SetActive(false);
            creditsEnglish.SetActive(true);
        }
    }
}