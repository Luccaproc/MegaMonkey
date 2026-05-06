using UnityEngine;

[System.Serializable]
public class SpriteLanguagePair
{
    public SpriteRenderer targetRenderer;
    public Sprite portugueseSprite;
    public Sprite englishSprite;
}

public class SpriteLanguageApplier : MonoBehaviour
{
    public SpriteLanguagePair[] spritePairs;

    void Start()
    {
        ApplyLanguage();
    }

    public void ApplyLanguage()
    {
        if (LanguageManager.Instance == null) return;

        string language = LanguageManager.Instance.CurrentLanguage;

        foreach (var pair in spritePairs)
        {
            if (pair.targetRenderer == null) continue;

            pair.targetRenderer.sprite =
                (language == "PT") ? pair.portugueseSprite : pair.englishSprite;
        }
    }
}