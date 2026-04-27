using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera[] allCameras;

    [Header("Y Damping Settings")]
    [SerializeField] private float fallPanAmount = 0.25f;
    [SerializeField] private float fallLerpSpeed = 5f;   // velocidade quando está caindo
    [SerializeField] private float riseLerpSpeed = 2f;   // velocidade quando volta ao normal

    public float fallSpeedYDampingChangeThreshold = -15f;

    private CinemachineCamera currentCamera;
    private CinemachinePositionComposer positionComposer;

    private float targetYDamping;
    private float normalYDamping;

    private void Start()
    {
        SetupCamera();
    }

    void SetupCamera()
    {
        for (int i = 0; i < allCameras.Length; i++)
        {
            if (allCameras[i] != null && allCameras[i].enabled)
            {
                currentCamera = allCameras[i];

                positionComposer = currentCamera
                    .GetCinemachineComponent(CinemachineCore.Stage.Body)
                    as CinemachinePositionComposer;

                break;
            }
        }

        if (positionComposer == null)
        {
            Debug.LogError("CameraManager: PositionComposer não encontrado!");
            return;
        }

        normalYDamping = positionComposer.Damping.y;
        targetYDamping = normalYDamping;
    }

    private void Update()
    {
        if (positionComposer == null) return;

        float currentYDamping = positionComposer.Damping.y;

        // escolhe velocidade dependendo do estado
        float speed = (targetYDamping == fallPanAmount) 
            ? fallLerpSpeed 
            : riseLerpSpeed;

        float newYDamping = Mathf.Lerp(
            currentYDamping,
            targetYDamping,
            Time.deltaTime * speed
        );

        positionComposer.Damping.y = newYDamping;
    }

    // 👉 chama isso do Player
    public void SetFalling(bool isFalling)
    {
        if (positionComposer == null) return;

        targetYDamping = isFalling ? fallPanAmount : normalYDamping;
    }
}