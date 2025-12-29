using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Transform heartsContainer;

    // Bu fonksiyon artık Update içinde değil, sadece can değişince çalışır
    public void UpdateHearts(int currentHealth)
    {
        // Container içindeki tüm çocukları (kalpleri) döngüye alıyoruz
        for (int i = 0; i < heartsContainer.childCount; i++)
        {
            // i değeri 0'dan başladığı için can miktarıyla kıyaslıyoruz
            if (i < currentHealth)
            {
                // Can varsa objeyi aktif et
                heartsContainer.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                // Can bittiyse objeyi tamamen gizle
                heartsContainer.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}