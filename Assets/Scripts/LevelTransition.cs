using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public int sceneIndex;
    private bool playerIsAtDoor = false;

    void Update()
    {
        if (playerIsAtDoor && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsAtDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsAtDoor = false;
        }
    }
}