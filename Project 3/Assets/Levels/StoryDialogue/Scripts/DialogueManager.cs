using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Scriptable Object")]
    [SerializeField] private GameMaster gameMaster;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterDialogue;
    [SerializeField] private Image character1Image;
    [SerializeField] private Image character2Image;

    [Header("Grey-ed Out Color")]
    [SerializeField] private Color greyedOut;

    private Queue<CharacterDialogue> dialogueQueue;

    private void OnEnable()
    {
        gameMaster.startDialogue += StartDialogue; 
    }

    private void OnDisable()
    {
        gameMaster.startDialogue -= StartDialogue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            NextDialogue(dialogueQueue.Dequeue());
        }
    }

    private void StartDialogue(Dialogue dialogue)
    {
        ConvertDialogue(dialogue);
        NextDialogue(dialogueQueue.Dequeue());
    }

    private void NextDialogue(CharacterDialogue characterDia)
    {
        characterName.text = characterDia.whoseTalking.name;
        characterDialogue.text = characterDia.sentence;
        character1Image.sprite = characterDia.characterLeft.portrait;
        character2Image.sprite = characterDia.characterRight.portrait;

        if (characterDia.characterLeft == characterDia.whoseTalking)
            character2Image.color = greyedOut;
        else
            character1Image.color = greyedOut;
    }

    #region Utility
    private void ConvertDialogue(Dialogue dialogue)
    {
        foreach (CharacterDialogue item in dialogue.dialogues)
            dialogueQueue.Enqueue(item);
    }
    #endregion
}
