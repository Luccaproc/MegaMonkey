using UnityEngine;
using System.Collections;


public class ProjectileLaunch : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform launchPoint;
    private Animator anim;

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
        if ((Input.GetKey(KeyCode.X) || Input.GetButtonDown("Fire1")) && shootCounter <= 0)
        {
            if (playerMovement.isRunning)
            {
                anim.SetTrigger("isShooting");
            }
            else
            {
                Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
            }
            shootCounter = shootTime;

        }
        shootCounter -= Time.deltaTime;
    }

    public void Shoot()
    {
        Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
    }
}
