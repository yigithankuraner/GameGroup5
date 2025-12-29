using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    public int goldAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        GoldManager gm = FindObjectOfType<GoldManager>();
        if (gm != null)
            gm.AddGold(goldAmount);

        Destroy(gameObject);
    }
}
