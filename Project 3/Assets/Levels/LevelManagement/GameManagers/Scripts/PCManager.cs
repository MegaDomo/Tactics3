using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPCManager", menuName = "Managers/PCManager")]
public class PCManager : ScriptableObject
{
    [Header("Player Characters")]
    public List<UnitObj> playersObjs;

    [Header("Defaults")]
    public List<UnitObj> defaultPlayerObjs;

    public void ResetPlayersToDefault()
    {
        playersObjs = defaultPlayerObjs;
    }
}
