using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotBar : MonoBehaviour
{
    [Header("Unity References")]
    public UIWeaponPanel weaponPanel;

    private PlayerTurn player;

    public void Setup()
    {
        player = PlayerTurn.instance;
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
        gameObject.SetActive(false);
        weaponPanel.SetActiveWeaponPanel(true);
    }

    public void Wait()
    {
        player.PlayerMove();
    }
    #endregion
}
