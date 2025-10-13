using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCollection", menuName = "Scriptable Objects/DialogueCollection")]
public class DialogueCollection : ScriptableObject
{
    public DialogueContainer[] dialogues;
}
