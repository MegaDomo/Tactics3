using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Units/Character")]
public class Character : ScriptableObject
{
    public new string name;
    public Sprite portrait;
    public Sprite fullBody;

    public Expression[] expressionSheet;
}
