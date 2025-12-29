using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public int sceneIndex;

    private bool playerIsAtDoor = false;
    private bool shopUsed = false;

    void Update()
    {
        // 🔒 Shop açıkken kapı hiçbir şey yapmaz
        if (ShopManager.Instance != null && ShopManager.Instance.IsShopOpen)
            return;

        if (!playerIsAtDoor) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            // 1️⃣ İlk E → Shop aç
            if (!shopUsed)
            {
                if (ShopManager.Instance != null)
                {
                    ShopManager.Instance.OpenShop();
                    shopUsed = true;
                }
            }
            // 2️⃣ İkinci E → Level değiştir
            else
            {
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsAtDoor = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsAtDoor = false;
    }
}
