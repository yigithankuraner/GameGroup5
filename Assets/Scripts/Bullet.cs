using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 1;
    public string ownerTag;

    private bool hasHit = false;

    void Start() { Destroy(gameObject, lifeTime); }

    void Update() { transform.Translate(Vector2.right * speed * Time.deltaTime); }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Kendi sahibine veya zaten çarptıysa işlem yapma
        if (hasHit || string.IsNullOrEmpty(ownerTag) || other.CompareTag(ownerTag))
            return;

        // --- OYUNCUYA ÇARPARSA ---
        if (other.CompareTag("Player"))
        {
            // 1. Önce eski sistemi kontrol et (PlayerHealth)
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                hasHit = true;
                health.TakeDamage(damage);
            }
            // 2. Eğer PlayerHealth yoksa YENİ sistemi kontrol et (PlayerStats)
            else
            {
                PlayerStats stats = PlayerStats.Instance; // Singleton üzerinden veya GetComponent ile
                if (stats == null) stats = other.GetComponent<PlayerStats>();

                if (stats != null)
                {
                    hasHit = true;
                    stats.TakeDamage(damage);
                }
            }
        }
        // --- DÜŞMANA ÇARPARSA ---
        else if (other.CompareTag("Enemy"))
        {
            bool hitSomething = false;

            // 1. Normal Düşman Kontrolü (Slime, Ghost vb.)
            EnemyHealth normalEnemy = other.GetComponent<EnemyHealth>();
            if (normalEnemy != null)
            {
                normalEnemy.TakeDamage(damage);
                hitSomething = true;
            }

            // 2. İSKELET BOSS KONTROLÜ (Bunu ekledik!)
            EvolvingSkeleton skeletonBoss = other.GetComponent<EvolvingSkeleton>();
            if (skeletonBoss != null)
            {
                skeletonBoss.TakeDamage(damage);
                hitSomething = true;
            }

            if (hitSomething) hasHit = true;
        }

        // Mermi bir şeye çarptı, yok et
        Destroy(gameObject);
    }
}