using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public UIHotBar hotbar;
    public UIWeaponPanel weaponPanel;
    public UITargetPanel targetPanel;

    private void Start()
    {
        hotbar.Setup();

        SetActiveHotBar(false);
        SetActiveWeaponPanel(false);
        SetActiveTargetPanel(false);
    }

    public void SetActiveHotBar(bool value)
    {
        if (value)
        {
            hotbar.enabled = value;
            hotbar.SetActiveHotBar(value);
        }
        else
        {
            hotbar.SetActiveHotBar(value);
            hotbar.enabled = value;
        }
    }
    public void SetActiveWeaponPanel(bool value)
    {
        if (value)
        {
            weaponPanel.enabled = value;
            weaponPanel.SetActiveWeaponPanel(value);
        }
        else
        {
            weaponPanel.SetActiveWeaponPanel(value);
            weaponPanel.enabled = value;
        }
    }
    public void SetActiveTargetPanel(bool value)
    {
        if (value)
        {
            targetPanel.enabled = value;
            targetPanel.SetActiveTargetPanel(value);
        }
        else
        {
            targetPanel.SetActiveTargetPanel(value);
            targetPanel.enabled = value;
        }
    }
}
