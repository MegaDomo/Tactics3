using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    [Header("Attributes")]
    public string name;
    public bool isTherePreDialogue;
    public Dialogue preCombatDialogue;
    public bool isTherePostDialogue;
    public Dialogue postCombatDialogue;

    [Header("Assets")]
    public GameObject map;
}
