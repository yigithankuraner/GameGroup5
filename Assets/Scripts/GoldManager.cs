using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    public int currentGold = 0;
    public TMP_Text goldText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() => UpdateUI();

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (goldText != null)
            goldText.text = currentGold.ToString();
    }
}
