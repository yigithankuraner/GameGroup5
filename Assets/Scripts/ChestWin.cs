using UnityEngine;

public class ChestWin : MonoBehaviour
{
    private bool isPlayerNearby = false;

    void Update()
    {
        // Oyuncu yakýndaysa ve E tuþuna basarsa
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (WinScreenManager.Instance != null)
            {
                WinScreenManager.Instance.ShowWinScreen(); // Zafer panelini açar
            }
            else
            {
                Debug.LogError("WinScreenManager sahnede bulunamadý!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Çarpanýn 'Player' tagine sahip olduðundan emin ol
        {
            isPlayerNearby = true;
            Debug.Log("Sandýða yaklaþtýn, 'E' tuþuna bas!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}