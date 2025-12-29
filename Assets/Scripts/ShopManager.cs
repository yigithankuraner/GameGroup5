using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("Panels")]
    public GameObject shopPanel;   // GlobalCanvas altındaki ShopPanel
    public GameObject hudPanel;    // GlobalCanvas altındaki HUD

    [Header("UI")]
    public TMP_Text shopGoldText;  // ShopPanel içindeki ShopGoldText

    [Header("Prices")]
    public int healthPrice = 10;
    public int damagePrice = 15;
    public int speedPrice = 12;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Shop ilk başta kapalı
        if (shopPanel != null) shopPanel.SetActive(false);

        // Gold event
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnGoldChanged += UpdateGoldUI;
            UpdateGoldUI(PlayerStats.Instance.gold);
        }
    }

    void OnDestroy()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.OnGoldChanged -= UpdateGoldUI;
    }

    void UpdateGoldUI(int gold)
    {
        if (shopGoldText != null)
            shopGoldText.text = gold.ToString();
    }

    public void OpenShop()
    {
        Debug.Log("SHOP AÇILDI (GLOBAL)");

        shopPanel.SetActive(true);
        shopPanel.transform.SetAsLastSibling(); // en üste al
        hudPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        hudPanel.SetActive(true);
        Time.timeScale = 1f;
    }

    public void BuyHealth()
    {
        if (!PlayerStats.Instance.SpendGold(healthPrice)) return;
        PlayerStats.Instance.maxHealth += 1;
    }

    public void BuyDamage()
    {
        if (!PlayerStats.Instance.SpendGold(damagePrice)) return;
        PlayerStats.Instance.damage += 1;
    }

    public void BuySpeed()
    {
        if (!PlayerStats.Instance.SpendGold(speedPrice)) return;
        PlayerStats.Instance.moveSpeed += 0.5f;
    }
}
