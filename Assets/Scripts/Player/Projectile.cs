using UnityEngine;

public class Projectile : MonoBehaviour
{
    EnemyHealth enemyHealth;
    public Rigidbody2D projectileRb;
    public float speed;
    public int damage;

    public float projectileLife;
    public float projectileCount;

    public PlayerMovement playerMovement;
    public bool facingRight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectileCount = projectileLife;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        facingRight = playerMovement.facingRight;
        if (!facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        projectileCount -= Time.deltaTime;
        if (projectileCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (facingRight)
        {
            projectileRb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            projectileRb.linearVelocity = new Vector2(-speed, 0);
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.tag == "Enemy")if (collision.gameObject.tag == "Enemy")
        {
            enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage);
        }
        
    }
}
