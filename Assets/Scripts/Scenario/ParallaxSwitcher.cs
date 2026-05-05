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

    void Start()
    {
        if (startWithSetA)
            ActivateSetA();
        else
            ActivateSetB();
    }

    // 🔁 Ativa conjunto A
    public void ActivateSetA()
    {
        SetVisible(parallaxSetA, true);
        SetVisible(parallaxSetB, false);
        usingSetA = true;
    }

    // 🔁 Ativa conjunto B
    public void ActivateSetB()
    {
        SetVisible(parallaxSetA, false);
        SetVisible(parallaxSetB, true);
        usingSetA = false;
    }

    // 👁️ Torna visível/invisível (inclui filhos)
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

    // 🎯 Trigger (executa só uma vez)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasSwitched)
        {
            if (usingSetA)
                ActivateSetB();
            else
                ActivateSetA();

            hasSwitched = true;
        }
    }
}