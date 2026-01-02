using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;

    // Bu deðiþken statiktir, yani sahne deðiþse bile hafýzada kalýr.
    // True ise: Oyun ilk kez açýlýyor (Menü göster).
    // False ise: Restart atýldý (Menüyü gösterme, oyunu baþlat).
    public static bool ShowMenuOnStart = true;

    void Awake()
    {
        if (mainMenuPanel == null) return;

        if (ShowMenuOnStart)
        {
            // Oyun ilk açýldý veya "Ana Menüye Dön" denildi.
            mainMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            // Restart atýldý, menüyü gizle ve oyunu akýt.
            mainMenuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // "New Game" butonuna baðlayacaðýn fonksiyon
    public void NewGame()
    {
        ShowMenuOnStart = false; // Artýk oyun baþladý
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // Öldüðünde veya Win ekranýnda "Main Menu" butonuna basýnca çalýþacak
    public void BackToMainMenu()
    {
        ShowMenuOnStart = true; // Menü açýlsýn istiyoruz
        Time.timeScale = 1f; // Sahne yüklenirken zaman aksýn
        SceneManager.LoadScene("Level1");
    }
}