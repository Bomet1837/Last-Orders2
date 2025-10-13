using UnityEngine;

[System.Serializable]
public class DialogueContainer
{
    [TextArea(3,10)]
    public string dialogue;
    public DialogueOption[] dialogueOptions;
}
