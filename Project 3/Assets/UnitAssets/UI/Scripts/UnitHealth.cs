using UnityEngine;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour
{
    [Header("Unity References")]
    public Player player;
    public Image fillImage;

    private void Start()
    {
        ChangeHealthBar(player.stats.curHealth, player.stats.maxHealth);
        player.healthChangeEvent.AddListener(ChangeHealthBar);
    }

    public void ChangeHealthBar(int curHealth, int maxHealth)
    {
        float cur = (float)curHealth;
        float max = (float)maxHealth;
        fillImage.fillAmount = cur / max;
    }
}
