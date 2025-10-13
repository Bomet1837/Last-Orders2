using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    [TextArea(1,10)]
    public string entryText;
    [TextArea(3, 10)]
    public string[] dialogues;
    public DialogueOption[] dialogueOptions;
}
