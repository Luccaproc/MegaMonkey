using UnityEngine;

public class ParallaxSwitcher : MonoBehaviour
{
    [Header("Parallax Sets")]
    public GameObject[] parallaxSetA;
    public GameObject[] parallaxSetB;

    [Header("Start Config")]
    public bool startWithSetA = true;

    private bool usingSetA;
    private bool hasSwitched = false;

    private const string SAVE_KEY = "ParallaxState";

    void Start()
    {
        // 🔥 carrega estado salvo
        int savedState = PlayerPrefs.GetInt(SAVE_KEY, startWithSetA ? 0 : 1);

        if (savedState == 0)
            ActivateSetA(false);
        else
            ActivateSetB(false);
    }

    // 🔁 Ativa Set A
    public void ActivateSetA(bool save = true)
    {
        SetVisible(parallaxSetA, true);
        SetVisible(parallaxSetB, false);

        usingSetA = true;

        if (save)
        {
            PlayerPrefs.SetInt(SAVE_KEY, 0);
            PlayerPrefs.Save();
        }
    }

    // 🔁 Ativa Set B
    public void ActivateSetB(bool save = true)
    {
        SetVisible(parallaxSetA, false);
        SetVisible(parallaxSetB, true);

        usingSetA = false;

        if (save)
        {
            PlayerPrefs.SetInt(SAVE_KEY, 1);
            PlayerPrefs.Save();
        }
    }

    // 🔥 NOVA FUNÇÃO:
    // inverte do B pro A
    public void SwitchBackToA()
    {
        ActivateSetA(true);
        hasSwitched = false;
    }

    // 👁️ Liga/desliga sprites
    private void SetVisible(GameObject[] set, bool value)
    {
        foreach (GameObject obj in set)
        {
            if (obj != null)
            {
                SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>();

                foreach (SpriteRenderer sr in renderers)
                {
                    sr.enabled = value;
                }
            }
        }
    }

    // 🎯 Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasSwitched)
        {
            ActivateSetB();
            hasSwitched = true;
        }
    }
}