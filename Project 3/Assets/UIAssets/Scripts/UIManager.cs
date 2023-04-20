using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Unity References")]
    public PlayerTurn playerTurn;
    public BattleSystem battleSystem;

    [Header("UI References")]
    public GameObject hotbar;
    public GameObject weaponPanel;
    public GameObject targetPanel;


    private void Start()
    {
        CloseAllPanels();
    }

    #region Events
    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void Setup()
    {
        hotbar = transform.GetChild(0).GetChild(0).gameObject;
        weaponPanel = transform.GetChild(0).GetChild(1).gameObject;
        targetPanel = transform.GetChild(0).GetChild(2).gameObject;
    }

    private void SubscribeEvents()
    {
        playerTurn.deselectedNodeEvent += CloseAllPanels;
        playerTurn.selectedNodeEvent += TurnOnHotBar;

        battleSystem.playerTurnEvent += CloseAllPanels;
    }

    private void UnsubscribeEvents()
    {
        playerTurn.deselectedNodeEvent -= CloseAllPanels;
        playerTurn.selectedNodeEvent -= TurnOnHotBar;

        battleSystem.playerTurnEvent -= CloseAllPanels;
    }
    #endregion

    public void LevelSelectButton()
    {
        SceneManager.LoadScene(0);
    }

    #region ChangePanels
    public void TurnOnHotBar()
    {
        SetActiveHotBar(true);
    }

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
        if (hotbar == null) Debug.Log("Yep It's Hotbar");
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
