using System.Collections;
using UnityEngine;

public class CameraFollowOBJECT : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Flip Rotation Status")]
    [SerializeField] private float flipYRotationTime;

    private PlayerMovement player;
    private bool facingRight;

    private bool hasInitialized = false;

    private void Awake()
    {
        if (playerTransform != null)
        {
            player = playerTransform.GetComponent<PlayerMovement>();
            facingRight = player.facingRight;
        }
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        // primeira vez: snap instantâneo
        if (!hasInitialized)
        {
            transform.position = playerTransform.position;
            hasInitialized = true;
            return;
        }

        // follow normal
        transform.position = playerTransform.position;
    }

    // 🔥 FUNÇÃO NOVA: força teleport da câmera
    public void SnapToPlayer()
    {
        if (playerTransform == null) return;

        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        LeanTween.rotateY(gameObject, DetermineEndRotation(), flipYRotationTime)
            .setEaseInOutCubic();
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        facingRight = !facingRight;
        return facingRight ? 180f : 0f;
    }
}