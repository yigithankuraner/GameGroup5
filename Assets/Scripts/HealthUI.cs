using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform heartContainer;

    // Kalpleri tuttuğumuz liste
    private List<GameObject> hearts = new List<GameObject>();

    // Start yerine OnEnable kullanıyoruz. 
    // Bu, obje her aktif olduğunda (Sahne açılışı dahil) çalışır.
    void OnEnable()
    {
        if (PlayerStats.Instance != null)
        {
            // Olay sistemine abone ol
            PlayerStats.Instance.OnHealthChanged += UpdateHearts;

            // ⚡ KRİTİK NOKTA: Abone olur olmaz ekranı zorla güncelle!
            // PlayerStats o an kaç can olduğunu biliyor, hemen çizsin.
            UpdateHearts(PlayerStats.Instance.currentHealth, PlayerStats.Instance.maxHealth);
        }
    }

    // Obje kapandığında aboneliği iptal et (Hata almamak için şart)
    void OnDisable()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthChanged -= UpdateHearts;
        }
    }

    void UpdateHearts(int current, int max)
    {
        // Güvenlik kontrolü
        if (heartContainer == null || heartPrefab == null) return;

        // 1. ADIM: Ekranda fazladan veya bozuk kalp kalmasın diye hepsini temizle
        // Mevcut listeyi ve sahnedeki objeleri sıfırlıyoruz.
        foreach (GameObject heart in hearts)
        {
            if (heart != null) Destroy(heart);
        }
        hearts.Clear();

        // Container içinde bizden habersiz oluşmuş (Editörden kalan) objeleri de temizle
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. ADIM: Maksimum can sayısı kadar yeni kalp oluştur
        for (int i = 0; i < max; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(newHeart);
        }

        // 3. ADIM: Mevcut cana göre kalpleri göster veya gizle
        for (int i = 0; i < hearts.Count; i++)
        {
            Image heartImage = hearts[i].GetComponent<Image>();

            if (heartImage != null)
            {
                // Eğer index, mevcut candan küçükse kalbi göster (Active/Enabled yap)
                // Örnek: Can 2 ise, 0. ve 1. kalp true olur, 2. kalp false olur.
                heartImage.enabled = i < current;
            }
        }
    }
}