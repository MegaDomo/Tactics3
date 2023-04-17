using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotBar : MonoBehaviour
{
    [Header("Scriptable Object References")]
    public PlayerTurn playerTurn;

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
