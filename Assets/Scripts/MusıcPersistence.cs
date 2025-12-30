using UnityEngine;

public class MusicPersistence : MonoBehaviour
{
    private static MusicPersistence instance;

    void Awake()
    {
        // Eğer bu objeden zaten bir tane varsa yenisini yok et
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            // Sahne değiştiğinde bu objeyi silme
            DontDestroyOnLoad(gameObject);
        }
    }
}