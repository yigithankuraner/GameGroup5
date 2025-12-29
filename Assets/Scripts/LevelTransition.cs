using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public int sceneIndex;

    private bool playerIsAtDoor;
    private bool shopUsed;

    void Update()
    {
        if (!playerIsAtDoor) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!shopUsed)
            {
                if (ShopManager.Instance != null)
                {
                    ShopManager.Instance.OpenShop();
                    shopUsed = true;
                }
                else
                {
                    Debug.LogError("ShopManager.Instance NULL! GlobalShopManager sahnede mi?");
                }
            }
            else
            {
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsAtDoor = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsAtDoor = false;
    }
}
