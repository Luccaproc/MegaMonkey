using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LanguageSpritePair
{
    public Image targetImage;
    public Sprite portugueseSprite;
    public Sprite englishSprite;
}

public class LanguageApplier : MonoBehaviour
{
    public LanguageSpritePair[] imagePairs;

    void Start()
    {
        ApplyLanguage();
    }

    public void ApplyLanguage()
    {
        if (LanguageManager.Instance == null) return;

        string language = LanguageManager.Instance.CurrentLanguage;

        foreach (var pair in imagePairs)
        {
            if (pair.targetImage == null) continue;

            pair.targetImage.sprite =
                (language == "PT") ? pair.portugueseSprite : pair.englishSprite;
        }
    }
}