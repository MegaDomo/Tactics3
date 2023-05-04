using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotBar : MonoBehaviour
{
    [Header("Scriptable Object References")]
    public PlayerTurn playerTurn;
    public BattleSystem battleSystem;

    [Header("UI References")]
    public GameObject hotBarElement;
    public List<AbilitySlot> slots;

    private void OnEnable()
    {
        playerTurn.selectedNodeEvent += UpdateHotBar;
        playerTurn.deselectedNodeEvent += CloseHotBar;

        battleSystem.playerTurnEvent += CloseHotBar;
    }

    private void OnDisable()
    {
        playerTurn.selectedNodeEvent -= UpdateHotBar;
        playerTurn.deselectedNodeEvent -= CloseHotBar;

        battleSystem.playerTurnEvent -= CloseHotBar;
    }

    private void UpdateHotBar(Unit player)
    {
        hotBarElement.SetActive(true);

        int len = player.abilities.Count;

        for (int i = 0; i < len; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].SetAbilityImage(player.abilities[i]);
        }
            
        for (int i = len; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
    }

    private void CloseHotBar(Unit Player)
    {
        hotBarElement.SetActive(false);
    }

    #region Event Handler
    public void WeaponStrike()
    {
        
    }

    public void Wait()
    {
        playerTurn.PlayerMove();
    }
    #endregion
}
