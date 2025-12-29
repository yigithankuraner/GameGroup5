using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolvingSkeleton : MonoBehaviour
{
    [Header("Settings")]
    public float detectionRange = 5f;
    public float moveSpeed = 2f;
    public float reviveTime = 4f;
    public int healthPhase1 = 2;
    public int healthPhase2 = 4;

    [Header("References")]
    public Transform player;
    public BoxCollider2D bodyCollider;

    private Animator anim;
    private Rigidbody2D rb;
    private bool isDead = true;
    private bool isRising = false;
    private bool isSecondPhase = false;
    private int currentHealth;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = healthPhase1;

        bodyCollider.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
    }

    void Update()
    {
        if (isDead)
        {
            HandleDormantState();
        }
        else if (!isRising)
        {
            HandleActiveState();
        }
    }

    void HandleDormantState()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange && !isRising)
        {
            StartCoroutine(ReviveRoutine());
        }
    }

    IEnumerator ReviveRoutine()
    {
        isRising = true;
        anim.SetTrigger("revive");

        yield return new WaitForSeconds(1f);

        rb.bodyType = RigidbodyType2D.Dynamic;

        isDead = false;
        isRising = false;
        bodyCollider.enabled = true;

        if (isSecondPhase)
            currentHealth = healthPhase2;
        else
            currentHealth = healthPhase1;
    }

    void HandleActiveState()
    {
        if (player == null) return;

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if ((direction > 0 && transform.localScale.x > 0) || (direction < 0 && transform.localScale.x < 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || isRising) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(DieAndPrepareNextPhase());
        }
    }

    IEnumerator DieAndPrepareNextPhase()
    {
        isDead = true;
        bodyCollider.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        anim.SetTrigger("die");

        if (!isSecondPhase)
        {
            isSecondPhase = true;
        }

        yield return new WaitForSeconds(reviveTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}