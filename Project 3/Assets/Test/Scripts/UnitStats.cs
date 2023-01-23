using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStats
{
    [Header("Stats")]
    public int movement;

    #region Background Stats
    [HideInInspector] public int moved;
    #endregion
}
