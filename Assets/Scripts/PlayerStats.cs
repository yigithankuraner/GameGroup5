using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Stats")]
    public int damage = 1;
    public float moveSpeed = 5f;

    [Header("Gold")]
    public int gold = 0;

    // EVENTS
    public event Action<int, int> OnHealthChanged; // current, max
    public event Action<int> OnGoldChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 🔴 SADECE OYUN İLK BAŞLARKEN
            currentHealth = maxHealth;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // ---------- HEALTH ----------

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;

        // ❗ SADECE EKLENEN KADAR CAN EKLE
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // ---------- GOLD ----------

    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }

    public bool SpendGold(int amount)
    {
        if (gold < amount)
            return false;

        gold -= amount;
        OnGoldChanged?.Invoke(gold);
        return true;
    }
}
