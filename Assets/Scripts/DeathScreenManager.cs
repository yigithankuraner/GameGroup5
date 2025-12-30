using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public static DeathScreenManager Instance;
    public GameObject deathScreen; // Inspector'da Game Over panelini buraya sürükle

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
            Time.timeScale = 0f; // Oyunu dondurur
            Debug.Log("Ölüm Ekranı Paneli Aktif Edildi!");
        }
        else
        {
            Debug.LogError("HATA: DeathScreenManager'da 'deathScreen' paneli atanmamış!");
        }
    }

    public void GoToMainMenu()
    {
        // 1. Ölüm ekranı panelini kapat
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }

        // 2. Zamanı normale döndür (Yoksa menüde hiçbir şeye basamazsın)
        Time.timeScale = 1f;

        // 3. Sahneyi yeniden yükle (Sahne yüklendiğinde MainMenuManager zaten menüyü açacak)
        SceneManager.LoadScene("Level1");
    }
}