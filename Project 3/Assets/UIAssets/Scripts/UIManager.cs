using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Unity References")]
    public PlayerTurn playerTurn;

    [Header("UI References")]
    public GameObject hotbar;
    public GameObject weaponPanel;
    public GameObject targetPanel;
    
    private void Start()
    {
        playerTurn.deselectedNodeEvent += CloseAllPanels;
        CloseAllPanels();
    }

    public void LevelSelectButton()
    {
        SceneManager.LoadScene(0);
    }

    #region ChangePanels
    public void ChangeFromHotbarToWeapon()
    {
        SetActiveHotBar(false);
        SetActiveWeaponPanel(true);
    }

    public void ChangeFromWeaponToTarget()
    {
        SetActiveWeaponPanel(false);
        SetActiveTargetPanel(true);
    }

    public void CloseTarget()
    {
        SetActiveTargetPanel(false);
    }

    public void CloseAllPanels()
    {
        SetActiveHotBar(false);
        SetActiveWeaponPanel(false);
        SetActiveTargetPanel(false);
    }

    public void CloseAllPanels(Unit unit)
    {
        SetActiveHotBar(false);
        SetActiveWeaponPanel(false);
        SetActiveTargetPanel(false);
    }
    #endregion

    #region Setters
    public void SetActiveHotBar(bool value)
    {
        hotbar.SetActive(value);
    }

    public void SetActiveWeaponPanel(bool value)
    {
        weaponPanel.SetActive(value);
    }

    public void SetActiveTargetPanel(bool value)
    {
        targetPanel.SetActive(value);
    }
    #endregion
}
