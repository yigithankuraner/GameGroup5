using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel; // Inspector'dan hazýrladýðýn paneli buraya sürükle
    void Awake()
    {
        // Sahne yüklendiði an paneli aktif et ve oyunu durdur
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true); // Menü ekranýný açar
            Time.timeScale = 0f; // Arkada oyunun baþlamasýný engeller
        }
    }

    void Start()
    {
        // Eðer oyuna direkt bu sahneden baþlýyorsak zamaný dondur ve menüyü göster
        if (mainMenuPanel != null && mainMenuPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
    }

    // NEW START butonu için fonksiyon
    public void NewGame()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false); // Menüyü kapat
            Time.timeScale = 1f; // Zamaný baþlat
        }
    }

    // Öldüðünde menüye dönmek için fonksiyon
    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // Zamaný normale döndür
        SceneManager.LoadScene("Level1"); // Sahneyi yeniden yükle
    }
}