using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public string CurrentLanguage { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CurrentLanguage = PlayerPrefs.GetString("Language", "PT");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPortuguese()
    {
        SetLanguage("PT");
    }

    public void SetEnglish()
    {
        SetLanguage("EN");
    }

    private void SetLanguage(string lang)
    {
        CurrentLanguage = lang;

        PlayerPrefs.SetString("Language", lang);
        PlayerPrefs.Save();

        ApplyLanguageToScene(); // 🔥 força atualização
    }

    private void ApplyLanguageToScene()
    {
        LanguageApplier[] appliers = FindObjectsOfType<LanguageApplier>(true);

        foreach (var applier in appliers)
        {
            applier.ApplyLanguage();
        }
    }
}