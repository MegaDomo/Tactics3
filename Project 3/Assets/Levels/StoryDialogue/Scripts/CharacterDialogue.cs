using UnityEngine;

[System.Serializable]
public class CharacterDialogue
{
    [Header("Characters")]
    public Character characterLeft;
    public Character characterRight;
    public Character whoseTalking;

    [Header("Details")]
    [TextArea(3, 10)] public string sentence;
}
