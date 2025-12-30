using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    public int goldAmount = 1;

    private void OnTriggerEnter2D(Collider2D other) // Buradaki isim 'other'
    {
        // 1. Çarpan þey oyuncu deðilse hiçbir þey yapma
        if (!other.CompareTag("Player")) return;

        // 2. Altýn miktarýný oyuncuya ekle
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddGold(goldAmount);
        }

        // 3. Altýn toplama sesini çal
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play(SoundManager.Instance.gold);
        }

        // 4. Altýn objesini yok et
        Destroy(gameObject);
    }
}