using UnityEngine;

public class Unparent : MonoBehaviour
{
    void Start()
    {
        // Remove o vínculo com o pai
        transform.SetParent(null);
    }
}