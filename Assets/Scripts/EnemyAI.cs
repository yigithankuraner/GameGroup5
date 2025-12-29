using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float patrolCheckDistance = 0.15f;
    public float flipCooldown = 0.4f;

    [Header("Combat")]
    public float detectionRange = 6f;
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;

    private int direction = 1; // 1 = sağ, -1 = sol
    private float nextFireTime;
    private float lastFlipTime;

    private Vector3 firePointStartPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // FirePoint başlangıç pozisyonu
        firePointStartPos = firePoint.localPosition;
    }

    void Update()
    {
        if (PlayerInSight())
        {
            StopAndShoot();
        }
        else
        {
            Patrol();
        }
    }

    // -------------------- PATROL --------------------
    void Patrol()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            patrolCheckDistance,
            groundLayer
        );

        if (!groundInfo && Time.time - lastFlipTime > flipCooldown)
        {
            Flip();
            lastFlipTime = Time.time;
        }

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    // -------------------- COMBAT --------------------
    void StopAndShoot()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (player == null) return;

        int targetDir = player.position.x > transform.position.x ? 1 : -1;

        if (targetDir != direction && Time.time - lastFlipTime > flipCooldown)
        {
            direction = targetDir;
            ApplyDirection();
            lastFlipTime = Time.time;
        }

        if (Time.time >= nextFireTime)
        {
            GameObject bullet = Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity
            );

            // Mermi kime ait
            bullet.GetComponent<Bullet>().ownerTag = "Enemy";

            // Sola bakıyorsa mermiyi çevir
            if (direction == -1)
                bullet.transform.rotation = Quaternion.Euler(0, 0, 180);

            nextFireTime = Time.time + fireRate;
        }
    }

    // -------------------- UTILS --------------------
    bool PlayerInSight()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    void Flip()
    {
        direction *= -1;
        ApplyDirection();
    }

    void ApplyDirection()
    {
        sr.flipX = direction == -1;

        // FirePoint'i aynala
        firePoint.localPosition = new Vector3(
            firePointStartPos.x * direction,
            firePointStartPos.y,
            firePointStartPos.z
        );
    }

    // -------------------- DEBUG --------------------
    void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            groundCheck.position,
            groundCheck.position + Vector3.down * patrolCheckDistance
        );
    }
}
