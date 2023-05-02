using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public enum DialogueState { InDialogue, OutOfDialogue }

    [Header("Scriptable Object")]
    [SerializeField] private GameMaster gameMaster;

    [Header("UI References")]
    [SerializeField] private GameObject UIElements;
    [SerializeField] private TextMeshProUGUI characterDialogue;
    [SerializeField] private GameObject leftCharacterNameObj;
    [SerializeField] private TextMeshProUGUI leftCharacterName;
    [SerializeField] private GameObject rightCharacterNameObj;
    [SerializeField] private TextMeshProUGUI rightCharacterName;
    [SerializeField] private Image[] charactersLeftImage;
    [SerializeField] private Image[] charactersRightImage;

    [Header("Grey-ed Out Color")]
    [SerializeField] private Color greyedOut;

    private Queue<CharacterDialogue> dialogueQueue = new Queue<CharacterDialogue>();

    private bool isTalking;
    private DialogueState dialogueState;

    private void OnEnable()
    {
        gameMaster.startDialogueEvent += StartDialogue; 
    }

    private void OnDisable()
    {
        gameMaster.startDialogueEvent -= StartDialogue;
    }

    private void Start()
    {
        UIElements.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5) && !isTalking && dialogueState == DialogueState.InDialogue)
        {
            ResetDialogue();
            
            gameMaster.StartCombat();
        }

        if (Input.GetKeyDown(KeyCode.F5) && isTalking)
        {
            NextDialogue(dialogueQueue.Dequeue());
        }
    }

    private void StartDialogue(Dialogue dialogue)
    {
        UIElements.SetActive(true);
        isTalking = true;
        dialogueState = DialogueState.InDialogue;
        ConvertDialogue(dialogue);
        NextDialogue(dialogueQueue.Dequeue());
    }

    private void NextDialogue(CharacterDialogue characterDia)
    {
        AdjustUIElements(characterDia);
        HighlightSpeaker(characterDia);

        if (dialogueQueue.Count == 0)
        {
            isTalking = false;
        }
    }

    #region Utility
    private void ConvertDialogue(Dialogue dialogue)
    {
        foreach (CharacterDialogue item in dialogue.dialogues)
            dialogueQueue.Enqueue(item);
    }

    private void ResetDialogue()
    {
        dialogueState = DialogueState.OutOfDialogue;
        UIElements.SetActive(false);
    }

    private void AdjustUIElements(CharacterDialogue characterDia)
    {
        characterDialogue.text = characterDia.sentence;

        // Left
        for (int i = 0; i < characterDia.charactersLeft.Length; i++)
        {
            charactersLeftImage[i].sprite = characterDia.charactersLeft[i].portrait;
            charactersLeftImage[i].color = greyedOut;
        }

        // Right
        for (int i = 0; i < characterDia.charactersRight.Length; i++)
        {
            charactersRightImage[i].sprite = characterDia.charactersRight[i].portrait;
            charactersRightImage[i].color = greyedOut;
        }
    }

    private void HighlightSpeaker(CharacterDialogue characterDia)
    {
        // Left
        for (int i = 0; i < characterDia.charactersLeft.Length; i++)
        {
            if (characterDia.charactersLeft[i] == characterDia.whoseTalking)
            {
                charactersLeftImage[i].color = Color.white;
                leftCharacterNameObj.SetActive(true);
                rightCharacterNameObj.SetActive(false);
                leftCharacterName.text = characterDia.whoseTalking.characterName;
                return;
            }
        }
        // Right
        for (int i = 0; i < characterDia.charactersRight.Length; i++)
        {
            if (characterDia.charactersRight[i] == characterDia.whoseTalking)
            {
                charactersRightImage[i].color = Color.white;
                rightCharacterNameObj.SetActive(true);
                leftCharacterNameObj.SetActive(false);
                rightCharacterName.text = characterDia.whoseTalking.characterName;
                return;
            }
        }
    }
    #endregion
}
