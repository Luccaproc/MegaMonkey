using UnityEngine;

public class LanguageUI : MonoBehaviour
{
    public void SetEnglish()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.SetEnglish();
    }

    public void SetPortuguese()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.SetPortuguese();
    }
}