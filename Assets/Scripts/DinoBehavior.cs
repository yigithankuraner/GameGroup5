using UnityEngine;

public class DinoAI : MonoBehaviour
{
    public float detectRange = 5f;
    public float chaseSpeed = 6f;
    public float patrolSpeed = 2f;
    public float stopDistance = 1.2f;

    public Transform pointA;
    public Transform pointB;

    public Animator anim;
    public Transform player;

    private bool facingRight = false;
    private Transform currentPatrolTarget;

    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        currentPatrolTarget = pointB;
    }

    void Update()
    {
        if (player == null || pointA == null || pointB == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectRange)
        {
            anim.SetBool("isAttacking", true);
            LookAtTarget(player.position.x);

            if (distanceToPlayer > stopDistance)
            {
                Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
            }
        }
        else
        {
            anim.SetBool("isAttacking", false);
            LookAtTarget(currentPatrolTarget.position.x);

            Vector2 patrolPos = new Vector2(currentPatrolTarget.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, patrolPos, patrolSpeed * Time.deltaTime);

            if (Mathf.Abs(transform.position.x - currentPatrolTarget.position.x) < 0.5f)
            {
                if (currentPatrolTarget == pointB)
                    currentPatrolTarget = pointA;
                else
                    currentPatrolTarget = pointB;
            }
        }
    }

    void LookAtTarget(float targetXPosition)
    {
        if (targetXPosition > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (targetXPosition < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}