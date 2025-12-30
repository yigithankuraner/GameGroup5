using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public static WinScreenManager Instance;
    public GameObject winScreen;

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

        // Sahne ilk açýldýðýnda panelin kapalý olduðundan emin ol
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
    }

    public void ShowWinScreen()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        // 1. Paneli kodla kapat (Kaybolmama sorununu bu çözer)
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }

        // 2. Zamaný normale döndür
        Time.timeScale = 1f;

        // 3. Sahneyi yükle
        SceneManager.LoadScene("Level1");
    }
}