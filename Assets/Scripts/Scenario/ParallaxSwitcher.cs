using UnityEngine;

public class ParallaxSwitcher : MonoBehaviour
{
    [Header("Parallax Sets")]
    public GameObject[] parallaxSetA;
    public GameObject[] parallaxSetB;

    [Header("Start Config")]
    public bool startWithSetA = true;

    [Header("Player Check")]
    public float requiredX = 1200f;

    private bool usingSetA;
    private bool hasSwitched = false;

    private Transform player;

    private const string SAVE_KEY = "ParallaxState";

    void Start()
    {
        // 🔥 pega automaticamente o player pela tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;

            Vector2 playerPos = player.position;

            if (playerPos.x >= requiredX)
            {
                ActivateSetB(false);
            }
            else
            {
                ActivateSetA(false);
            }
        }
        else
        {
            // fallback se não achar player
            int savedState = PlayerPrefs.GetInt(SAVE_KEY, startWithSetA ? 0 : 1);

            if (savedState == 0)
                ActivateSetA(false);
            else
                ActivateSetB(false);
        }
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasSwitched)
        {
            ActivateSetB();
            hasSwitched = true;
        }
    }

    public void ResetToOriginal()
    {
        ActivateSetA(true);
        hasSwitched = false;
    }
}