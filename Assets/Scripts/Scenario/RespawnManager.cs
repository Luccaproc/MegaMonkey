using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    public static Vector3? savedRespawnPosition = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(RespawnWhenReady());
    }

    private IEnumerator RespawnWhenReady()
    {
        GameObject player = null;

        // 🔥 espera até o player existir
        while (player == null)
        {
            player = GameObject.FindWithTag("Player");
            yield return null;
        }

        // 🔥 se ainda não tiver checkpoint, usa o starting point depois
        if (savedRespawnPosition == null)
            yield break;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.position = savedRespawnPosition.Value;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            player.transform.position = savedRespawnPosition.Value;
        }
    }

    public void SetRespawnPoint(Transform newPoint)
    {
        savedRespawnPosition = newPoint.position;
    }
}