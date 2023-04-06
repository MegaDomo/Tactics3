using UnityEngine;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour
{
    [Header("Unity References")]
    public Player player;
    public Image fillImage;

    private void Start()
    {
        player.healthChangeEvent.AddListener(ChangeHealthBar);
    }

    public void ChangeHealthBar(int curHealth, int maxHealth)
    {
        fillImage.fillAmount = curHealth / maxHealth;
    }
}
