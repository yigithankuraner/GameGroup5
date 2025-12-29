using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Invincibility")]
    public float invincibilityDuration = 0.2f;
    private float nextDamageTime;

    [Header("Visual")]
    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
    }

    // DÜŞMAN / MERMİ vs. BURAYI ÇAĞIRIR
    public void TakeDamage(int damage)
    {
        if (Time.time < nextDamageTime)
            return;

        if (PlayerStats.Instance == null)
            return;

        nextDamageTime = Time.time + invincibilityDuration;

        // HASARI GERÇEKTEN BURADA VERİYORUZ
        PlayerStats.Instance.TakeDamage(damage);

        StopAllCoroutines();
        StartCoroutine(HitFlash());

        Debug.Log("Player HP: " + PlayerStats.Instance.currentHealth);

        if (PlayerStats.Instance.currentHealth <= 0)
            Die();
    }

    IEnumerator HitFlash()
    {
        if (sr == null) yield break;

        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    void Die()
    {
        Debug.Log("PLAYER DEAD");

        // Şimdilik oyunu durdur
        Time.timeScale = 0f;

        // İleride:
        // Respawn
        // Game Over UI
        // Save / Load
    }
}
