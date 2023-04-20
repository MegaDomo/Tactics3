using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    [Header("Attributes")]
    public string name;
    public bool isThereDialogue;
    public Dialogue dialogue;

    [Header("Assets")]
    public GameObject map;
}
