using UnityEngine;
public class DestroyEffect : MonoBehaviour
{
    void Start()
    {
        // Efekti 0.5 saniye sonra (animasyon bitince) yok et
        Destroy(gameObject, 0.5f);
    }
}