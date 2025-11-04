using UnityEngine;
using System.Collections;


public class ProjectileLaunch : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform launchPoint;
    private Animator anim;
   // public PlayerMovement playerMovement;

    public float shootTime;
    public float shootCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootCounter = shootTime;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && shootCounter <= 0)
        {
            Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
            shootCounter = shootTime;

            //Animação
            anim.SetTrigger("isShooting");
        }
        shootCounter -= Time.deltaTime;
    }

    private IEnumerator ResetShootAnimation()
    {
        yield return new WaitForSeconds(0.2f); // tempo da animação de tiro
        anim.SetBool("isShooting", false);
        Debug.Log("Foi");
    }
}
