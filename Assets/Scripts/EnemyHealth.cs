using UnityEngine;
using TMPro;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int currentHealth;

    public int goldReward = 5;
    public GameObject goldPopupPrefab;
    public float popupYOffset = 0.3f;

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StopAllCoroutines();
        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    void Die()
{
    if (GoldManager.Instance != null)
        GoldManager.Instance.AddGold(goldReward);

    // Popup oluştur
    GameObject popup = Instantiate(
        goldPopupPrefab,
        transform.position,
        Quaternion.identity
    );

    // Geçici olarak enemy'ye bağla
    popup.transform.SetParent(transform);
    popup.transform.localPosition = new Vector3(0f, popupYOffset, 0f);

    // Sonra AYIR (kritik nokta)
    popup.transform.SetParent(null);

    // Yazıyı ayarla
    TMP_Text t = popup.GetComponent<TMP_Text>();
    if (t != null)
        t.text = "+" + goldReward;

    // Enemy'yi yok et
    Destroy(gameObject);
}
}
