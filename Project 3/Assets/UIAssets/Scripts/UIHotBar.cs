using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotBar : MonoBehaviour
{
    [Header("Unity References")]
    public UIWeaponPanel weaponPanel;

    private PlayerTurn player;

    private void Start()
    {
        player = PlayerTurn.instance;
        SetActiveHotBar(false);
    }

    #region Setters
    public void SetActiveHotBar(bool value)
    {
        gameObject.SetActive(value);
    }
    #endregion

    #region Event Handler
    public void WeaponStrike()
    {
        weaponPanel.SetActiveWeaponPanel(true);
    }

    public void Wait()
    {
        player.PlayerMove();
    }
    #endregion
}
