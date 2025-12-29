using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform heartContainer;

    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        // 🔴 SAHNEDE VAR OLAN KALPLERİ TEMİZLE
        hearts.Clear();

        for (int i = heartContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(heartContainer.GetChild(i).gameObject);
        }

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthChanged += UpdateHearts;
            UpdateHearts(
                PlayerStats.Instance.currentHealth,
                PlayerStats.Instance.maxHealth
            );
        }
    }

    void OnDestroy()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.OnHealthChanged -= UpdateHearts;
    }

    void UpdateHearts(int current, int max)
    {
        while (hearts.Count > max)
        {
            Destroy(hearts[hearts.Count - 1]);
            hearts.RemoveAt(hearts.Count - 1);
        }

        while (hearts.Count < max)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(heart);
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            Image img = hearts[i].GetComponent<Image>();
            img.enabled = i < current;
        }
    }
}
