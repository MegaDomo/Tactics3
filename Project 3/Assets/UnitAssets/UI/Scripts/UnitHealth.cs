using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitHealth : MonoBehaviour
{
    [Header("Unity References")]
    public Player player;
    public Image portraitImage;
    public Image fillImage;
    public TextMeshProUGUI healthText;

    private void Start()
    {
        ChangeHealthBar(player.stats.curHealth, player.stats.maxHealth);
        portraitImage.sprite = player.portrait;
        player.healthChangeEvent.AddListener(ChangeHealthBar);
    }

    public void ChangeHealthBar(int curHealth, int maxHealth)
    {
        float cur = curHealth;
        float max = maxHealth;
        fillImage.fillAmount = cur / max;
        healthText.text = cur.ToString() + " / " + max.ToString();
    }
}
