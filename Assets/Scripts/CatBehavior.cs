using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellGatoAI : MonoBehaviour
{
    public Vector3 forcedScale = new Vector3(5f, 5f, 1f);
    public bool spriteFacesLeft = true;

    [Header("Settings")]
    public float detectionRange = 6f;
    public float tooCloseRange = 2.5f;
    public float attackJumpForce = 8f;
    public float retreatJumpForce = 5f;
    public float prepareDuration = 0.8f;
    public float recoverDuration = 1.5f;
    public int damageAmount = 1;

    [Header("References")]
    public Transform player;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private float stateTimer;
    private bool isGrounded;
    private bool isFacingRight = true;

    private enum State { Idle, Preparing, Attacking, Retreating, Recovering }
    private State currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentState = State.Idle;

        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (spriteFacesLeft) isFacingRight = false;
    }

    void Update()
    {
        CheckGround();
        HandleSpriteFlip();

        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Preparing:
                HandlePreparing();
                break;
            case State.Attacking:
                HandleAttacking();
                break;
            case State.Retreating:
                HandleRetreating();
                break;
            case State.Recovering:
                HandleRecovering();
                break;
        }

        UpdateAnimator();
    }

    void LateUpdate()
    {
        Vector3 finalScale = forcedScale;

        if (isFacingRight)
        {
            finalScale.x = Mathf.Abs(forcedScale.x);
        }
        else
        {
            finalScale.x = -Mathf.Abs(forcedScale.x);
        }

        if (spriteFacesLeft)
        {
            finalScale.x *= -1;
        }

        transform.localScale = finalScale;
    }

    void HandleIdle()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < tooCloseRange)
        {
            StartRetreat();
        }
        else if (distanceToPlayer < detectionRange)
        {
            StartPrepare();
        }
    }

    void StartPrepare()
    {
        currentState = State.Preparing;
        stateTimer = prepareDuration;
        rb.linearVelocity = Vector2.zero;
    }

    void HandlePreparing()
    {
        stateTimer -= Time.deltaTime;

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer < tooCloseRange)
            {
                StartRetreat();
                return;
            }
        }

        if (stateTimer <= 0)
        {
            PerformAttack();
        }
    }

    void StartRetreat()
    {
        if (player == null) return;

        currentState = State.Retreating;
        Vector2 direction = (transform.position - player.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * retreatJumpForce, retreatJumpForce * 0.5f);
    }

    void HandleRetreating()
    {
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            currentState = State.Preparing;
            stateTimer = prepareDuration;
        }
    }

    void PerformAttack()
    {
        if (player == null) return;

        currentState = State.Attacking;
        Vector2 direction = (player.position - transform.position).normalized;

        float jumpHeight = 0.5f;
        if (player.position.y > transform.position.y)
            jumpHeight = 0.8f;

        rb.AddForce(new Vector2(direction.x * attackJumpForce, attackJumpForce * jumpHeight), ForceMode2D.Impulse);
    }

    void HandleAttacking()
    {
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            currentState = State.Recovering;
            stateTimer = recoverDuration;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void HandleRecovering()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            currentState = State.Idle;
        }
    }

    void CheckGround()
    {
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void HandleSpriteFlip()
    {
        if (player == null) return;

        if (currentState == State.Recovering || currentState == State.Preparing || currentState == State.Idle)
        {
            if (player.position.x > transform.position.x && !isFacingRight)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && isFacingRight)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
    }

    void UpdateAnimator()
    {
        anim.SetBool("isPreparing", currentState == State.Preparing);
        anim.SetBool("isJumping", !isGrounded);
        anim.SetBool("isRecovering", currentState == State.Recovering);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && currentState == State.Attacking)
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDir * 5f, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tooCloseRange);
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
}