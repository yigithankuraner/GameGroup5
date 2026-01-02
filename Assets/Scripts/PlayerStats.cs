using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Başlangıç Ayarları")]
    public int defaultMaxHealth = 3;
    public float defaultMoveSpeed = 5f;
    public int defaultDamage = 1; // Hasar ayarı eklendi

    [Header("Anlık Durum")]
    public int maxHealth;
    public int currentHealth;
    public float moveSpeed;
    public int damage; // ❗ Eksik olan değişken buydu, geri geldi.
    public int gold = 0;

    // EVENTS
    public event Action<int, int> OnHealthChanged; // current, max
    public event Action<int> OnGoldChanged;        // gold

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // İlk açılışta ayarları yükle
            maxHealth = defaultMaxHealth;
            currentHealth = maxHealth;
            moveSpeed = defaultMoveSpeed;
            damage = defaultDamage;
            gold = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Invoke("ForceUIUpdate", 0.1f);
    }

    void ForceUIUpdate()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnGoldChanged?.Invoke(gold);
    }

    // --- OYUNU SIFIRLAMA (Ölünce Restart Atınca) ---
    public void ResetGame()
    {
        maxHealth = defaultMaxHealth;
        currentHealth = maxHealth;
        damage = defaultDamage; // Hasarı da sıfırlıyoruz
        moveSpeed = defaultMoveSpeed;
        gold = 0;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnGoldChanged?.Invoke(gold);
    }

    // --- SAĞLIK SİSTEMİ ---
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // --- EKSİK OLAN PARÇA BU ---
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // Can bittiği an Ölüm Ekranı Yöneticisine haber ver
            if (DeathScreenManager.Instance != null)
            {
                DeathScreenManager.Instance.ShowDeathScreen();
            }
        }
        // ---------------------------

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // --- ALTIN SİSTEMİ ---
    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }

    public bool SpendGold(int amount)
    {
        if (gold < amount)
        {
            return false;
        }

        gold -= amount;
        OnGoldChanged?.Invoke(gold);
        return true;
    }
}