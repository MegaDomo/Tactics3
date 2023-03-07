using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotBarTest : MonoBehaviour
{
    [Header("Unity References")]
    public GameObject hotBar;
    public WeaponPanel weaponPanel;

    private PlayerTurn player;

    #region Hotbar Handling
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
    #endregion

    #region Event Handler
    public void WeaponStrike()
    {
        List<Node> neighbors = MapManager.instance.pathing.GetNeighbors(player.GetDestination());
        List<Unit> targets = new List<Unit>();
        foreach (Node node in neighbors)
        {
            if (node.unit != null)
                targets.Add(node.unit);
        }

        if (targets.Count == 0)
            return;
        Unit selected = player.GetSelected();

        targets[0].TakePhysicalDamage(selected.equippedWeapon.damage);
    }

    public void Wait()
    {
        player.PlayerMove();
    }
    #endregion
}
