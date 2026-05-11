using UnityEngine;

public class FinalResetSpawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject startingSpawn =
                GameObject.FindGameObjectWithTag("FirstSpawn");

            if (startingSpawn != null)
            {
                RespawnManager.Instance.SetRespawnPoint(
                    startingSpawn.transform
                );
            }
        }
        
    }
}