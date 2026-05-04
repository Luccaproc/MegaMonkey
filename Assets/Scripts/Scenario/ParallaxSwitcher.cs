using UnityEngine;

public class ParallaxSwitcher : MonoBehaviour
{
    [Header("Parallax Sets")]
    public GameObject[] parallaxSetA;
    public GameObject[] parallaxSetB;

    [Header("Start Config")]
    public bool startWithSetA = true;

    private bool usingSetA;

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
        SetActive(parallaxSetA, true);
        SetActive(parallaxSetB, false);
        usingSetA = true;
    }

    // 🔁 Ativa conjunto B
    public void ActivateSetB()
    {
        SetActive(parallaxSetA, false);
        SetActive(parallaxSetB, true);
        usingSetA = false;
    }

    // 🔧 Liga/desliga objetos
    private void SetActive(GameObject[] set, bool value)
    {
        foreach (GameObject obj in set)
        {
            if (obj != null)
                obj.SetActive(value);
        }
    }

    // 🎯 Trigger automático
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (usingSetA)
                ActivateSetB();
            else
                ActivateSetA();
        }
    }
}