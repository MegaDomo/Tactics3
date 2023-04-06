using System;
using UnityEngine;
using TMPro;

public class LevelSelectPlayerUI : MonoBehaviour
{
    [Header("Unity References")]
    public Player player;
    public TextMeshProUGUI hpInput;

    public void SetPlayerObject()
    {
        if (hpInput.text == String.Empty)
            return;
        if (!int.TryParse(hpInput.text, out int x))
            return;
        player.SetHealth(x);
    }
}
