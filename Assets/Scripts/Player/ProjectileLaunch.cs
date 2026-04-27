using UnityEngine;
using System.Collections;


public class ProjectileLaunch : MonoBehaviour
{
    [Header("Shoot Inputs")]
    private PlayerControls playerControls;
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    void Awake()
    {
        playerControls = new PlayerControls();
    }
    [Header("Shoot Variables")]
    public GameObject projectilePrefab;
    public Transform launchPoint;
    public float shootTime;
    public float shootCounter;

    [Header("Animation")]
    [SerializeField] private PlayerMovement playerMovement;
    private Animator anim;


    [Header("Sounds")]
    public AudioSource shootSound;
    // public PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootCounter = shootTime;
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = playerMovement.Anim.GetCurrentAnimatorStateInfo(0);

        if (playerControls.Player.Shoot1.IsPressed() && shootCounter <= 0)
        {
            Shoot();
            shootCounter = shootTime;
        }
        shootCounter -= Time.deltaTime;
    }

    public void Shoot()
    {
        Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        shootSound.Play();
    }
}
