using UnityEngine;
using System.Collections;


public class ProjectileLaunch : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform launchPoint;
    private Animator anim;
    public AudioSource shootSound;

    PlayerMovement playerMovement;
   // public PlayerMovement playerMovement;

    public float shootTime;
    public float shootCounter;
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
        if ((Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Mouse0)) && shootCounter <= 0)
        {
            if (playerMovement.onGround)
            {
                if (playerMovement.isRunning)
                {
                    anim.SetTrigger("isShooting");
                }
                else
                {
                    Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
                    shootSound.Play();
                }
                shootCounter = shootTime;
            }
        }
        shootCounter -= Time.deltaTime;
    }

    public void Shoot()
    {
        Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        shootSound.Play();
    }
}
