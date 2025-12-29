using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public HealthUI healthUI;

    private float nextDamageTime;
    private float invincibilityDuration = 0.2f;

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        maxHealth = PlayerStats.Instance.maxHealth;
        currentHealth = maxHealth;

        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (Time.time < nextDamageTime) return;

        currentHealth -= damage;
        nextDamageTime = Time.time + invincibilityDuration;

        StopAllCoroutines();
        StartCoroutine(HitFlash());   // KIRMIZI OL

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    void Die()
    {
        Debug.Log("PLAYER DEAD");
        Time.timeScale = 0f;
    }
}
