using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Gold")]
    public int gold = 0;

    [Header("Stats")]
    public int maxHealth = 3;
    public int damage = 1;
    public float moveSpeed = 5f;

    // EVENT
    public event Action<int> OnGoldChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // GOLD EKLE
    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }

    // GOLD HARCA
    public bool SpendGold(int amount)
    {
        if (gold < amount)
            return false;

        gold -= amount;
        OnGoldChanged?.Invoke(gold);
        return true;
    }
}
