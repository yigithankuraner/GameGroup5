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

    public void RestartGame()
    {
        Time.timeScale = 1f; // Zamanı normale döndürmezsen yeni sahne de donuk olur
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}