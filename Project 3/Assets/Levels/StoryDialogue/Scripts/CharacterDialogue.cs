using UnityEngine;

[System.Serializable]
public class CharacterDialogue
{
    [Header("Characters")]
    public UnitObj[] charactersLeft;
    public UnitObj[] charactersRight;
    public UnitObj whoseTalking;

    [Header("Details")]
    [TextArea(3, 10)] public string sentence;
}
