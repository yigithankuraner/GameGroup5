using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public static DeathScreenManager Instance;
    public GameObject deathScreen;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDeathScreen()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartLevel()
    {
        // 1. Paneli Kapat
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }

        // 2. STATLARI SIFIRLA (Max can 3 olsun, altın gitsin vb.)
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ResetGame();
        }

        // 3. Zamanı düzelt
        MainMenuManager.ShowMenuOnStart = false;
        Time.timeScale = 1f;

        // 4. LEVEL 1'e DÖN (Level 2'de doğmamak için)
        // Eğer kaldığın levelden devam etmek istersen burayı "SceneManager.GetActiveScene().name" yapabilirsin.
        SceneManager.LoadScene("Level1");
    }
}