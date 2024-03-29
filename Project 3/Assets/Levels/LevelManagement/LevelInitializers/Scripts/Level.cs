using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    [Header("Attributes")]
    public string name;

    [Header("Enemies")]
    public List<UnitObj> section1Enemies;
    public List<UnitObj> section2Enemies;
    public List<UnitObj> section3Enemies;

    [Header("Dialogue")]
    public bool isTherePreDialogue;
    public Dialogue preCombatDialogue;
    public bool isTherePostDialogue;
    public Dialogue postCombatDialogue;

    [Header("Assets")]
    public GameObject map;
}
