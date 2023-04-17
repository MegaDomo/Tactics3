using System;
using System.Globalization;
using UnityEngine;
using TMPro;

public class LevelSelectPlayerUI : MonoBehaviour
{
    [Header("Unity References")]
    public LevelSelector levelSelector;
    public Unit player;

    [Header("UI References")]
    public TMP_InputField field;

    private void Start()
    {
        levelSelector.selectLevelEvent += SetPlayerObject;    
    }

    public void SetPlayerObject()
    {
        if (field.text == String.Empty)
            return;

        int x = 0;
        string value = field.text;
        Debug.Log(value);
        if (!int.TryParse(value, out x))
        {
            Debug.Log("Here");
            return;
        }
            
            
        player.SetHealth(x);
    }
}
