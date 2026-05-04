using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] apples;

    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < apples.Length; i++)
        {
            apples[i].enabled = i < currentHealth;
        }
    }
}