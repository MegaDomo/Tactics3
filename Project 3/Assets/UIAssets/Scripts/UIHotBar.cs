using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotBar : MonoBehaviour
{
    private PlayerTurn player;

    private void Start()
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
        
    }

    public void Wait()
    {
        player.PlayerMove();
    }
    #endregion
}
