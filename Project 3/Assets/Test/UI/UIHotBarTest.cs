using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotBarTest : MonoBehaviour
{
    [Header("Unity References")]
    public GameObject hotBar;

    private PlayerTurn player;

    private void Start()
    {
        player = PlayerTurn.instance;
        TurnOffHotBar();
    }

    private void Update()
    {
        if (CanPlayerChooseAction())
            TurnOnHotBar();
        else
            TurnOffHotBar();
    }

    private bool CanPlayerChooseAction()
    {
        if (CombatState.state == BattleState.PLAYERTURN && player.actionState == ActionState.ChoosingAction)
            return true;
        return false;
    }

    private void TurnOffHotBar()
    {
        if (!hotBar.activeInHierarchy)
            return;
        hotBar.SetActive(false);
    }
    private void TurnOnHotBar()
    {
        if (hotBar.activeInHierarchy)
            return;
        hotBar.SetActive(true);
    }

    #region Event Handler
    public void WeaponStrike()
    {
        /*if (player.actionState == ActionState.ChoosingAction)
        {
            player.actionState = ActionState.ChoosingTarget;
        }*/
    }

    public void Wait()
    {
        player.PlayerMove();
    }
    #endregion
}