using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class CameraFollowOBJECT : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Flip Rotation Status")]
    [SerializeField] private float flipYRotationTime = 0.5f;

    private Coroutine turnCoroutine;

    private PlayerMovement player;

    private bool facingRight;

    private void Awake()
    {
        player = playerTransform.gameObject.GetComponent<PlayerMovement>();

        facingRight = player.facingRight;
    }

    // Update is called once per frame
    private void Update()
    {
        //Faz o CameraFollowOBJECT seguir o player
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        LeanTween.rotateY(gameObject, DetermineEndRotation(), flipYRotationTime).setEaseInOutSine();
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

        if (facingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
}
