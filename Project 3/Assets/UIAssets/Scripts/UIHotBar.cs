using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotBar : MonoBehaviour
{
    [Header("Scriptable Object References")]
    public PlayerTurn playerTurn;

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
        playerTurn.PlayerMove();
    }
    #endregion
}
