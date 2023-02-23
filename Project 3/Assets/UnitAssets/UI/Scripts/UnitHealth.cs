using UnityEngine;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour
{
    [Header("Unity References")]
    public Image fillImage;

    private UnitStats stats;

    private void Start()
    {
        stats = GetComponent<Unit>().stats;
    }

    private void Update()
    {
        fillImage.fillAmount = stats.curHealth / stats.maxHealth;
    }
}
